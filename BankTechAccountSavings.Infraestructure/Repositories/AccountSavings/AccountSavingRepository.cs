using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankTechAccountSavings.Infraestructure.Repositories.AccountSavings
{
    public class AccountSavingRepository(AccountSavingDbContext context) : IAccountSavingRepository
    {
        private readonly AccountSavingDbContext _context = context;

        public async Task<AccountSaving> CreateAsync(AccountSaving entity, CancellationToken cancellationToken)
        {
            entity.AccountNumber = GenerateBankAccountNumber();

            while (await _context.Set<AccountSaving>().AnyAsync(e => e.AccountNumber == entity.AccountNumber, cancellationToken))
            {
                entity.AccountNumber = GenerateBankAccountNumber();
            }

            entity.AccountName ??= "CTA.AHORROS";
            entity.Bank = "BankTech";
            entity.CurrentBalance = entity.CurrentBalance;
            entity.AnnualInterestRate = 0.30m / 100;
            entity.MonthlyInterestGenerated = 0;
            entity.Currency = entity.Currency != 0 ? entity.Currency : Currency.DOP;
            entity.DateOpened = DateTime.UtcNow.Date;
            entity.AccountStatus = AccountStatus.Active;

            await _context.Set<AccountSaving>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Deposit?> AddDepositAsync(int amount, Guid accountId, string description, CancellationToken cancellationToken)
        {
            var account = await _context.AccountSavings.FindAsync(new object[] { accountId }, cancellationToken);
            if (account == null) return null;
            if (amount <= 0)
            {
                throw new InvalidOperationException("Invalid Deposit Amount");
            }
            account.CurrentBalance += amount;

            Deposit newDeposit = new()
            {
                DestinationProduct = account,
                DestinationProductId = account.Id,
                Credit = amount,
                TransactionDate = DateTime.UtcNow.Date,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = description,
                TransactionType = TransactionType.Deposit,
                TransactionStatus = TransactionStatus.Completed,
                Amount = amount,
            };
            account.Deposits.Add(newDeposit);
            await _context.Set<Deposit>().AddAsync(newDeposit, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return account.Deposits.LastOrDefault();
        }

        public async Task<Withdraw?> WithDrawAsync(int amount, Guid accountId, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(new object[] { accountId }, cancellationToken);
            if (account == null) return null;
            if (account.CurrentBalance < amount)
            {
                throw new InvalidOperationException("Insufficient Funds");
            }
            else if (amount <= 0)
            {
                throw new InvalidOperationException("Invalid withdraw amount");
            }

            Withdraw newWithdraw = new()
            {
                SourceProduct = account,
                SourceProductId = account.Id,
                Debit = amount,
                Amount = amount,
                TransactionDate = DateTime.UtcNow.Date,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = $"Retiro el día:{DateTime.UtcNow.Date}",
                TransactionType = TransactionType.WithDraw,
                TransactionStatus = TransactionStatus.Completed,
                Tax = (0.0015 * amount),
                Total = (amount + 100 + (0.0015 * amount))
            };
            account.CurrentBalance -= (decimal)newWithdraw.Total;
            await _context.Set<Withdraw>().AddAsync(newWithdraw, cancellationToken);
            account.WithDraws.Add(newWithdraw);
            await _context.SaveChangesAsync(cancellationToken);
            return account.WithDraws.LastOrDefault();
        }

        public async Task<Transfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transferType, CancellationToken cancellationToken)
        {
            AccountSaving? fromAccount = await _context.Set<AccountSaving>().FindAsync(new object[] { fromAccountId }, cancellationToken);
            AccountSaving? toAccount = await _context.Set<AccountSaving>().FindAsync(new object[] { toAccountId }, cancellationToken);

            if (fromAccount == null || toAccount == null)
            {
                throw new InvalidOperationException("Account not found");
            }

            if (transferAmount <= 0 || transferAmount > fromAccount.CurrentBalance)
            {
                throw new InvalidOperationException("Invalid transfer amount or insufficient funds");
            }

            int commission = (transferType == TransferType.LBTR) ? 100 : 0;

            Transfer newTransfer = new()
            {
                SourceProduct = fromAccount,
                SourceProductId = fromAccount.Id,
                DestinationProduct = toAccount,
                DestinationProductId = toAccount.Id,
                TransferType = transferType,
                TransactionType = TransactionType.Transfer,
                Description = description,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                TransactionStatus = TransactionStatus.Completed,
                Amount = transferAmount,
                Commission = commission,
                Tax = 0.0015 * transferAmount,
                Total = transferAmount + 100 + (0.0015 * transferAmount),
                Credit = transferAmount,
                Debit = transferAmount
            };

            fromAccount.CurrentBalance -= (decimal)newTransfer.Total;
            toAccount.CurrentBalance += transferAmount;

            fromAccount.TransfersAsSource.Add(newTransfer);
            toAccount.TransfersAsDestination.Add(newTransfer);

            await _context.SaveChangesAsync(cancellationToken);

            return newTransfer;
        }

        public async Task<AccountSaving?> CloseAccountSavingAsync(Guid accountId, CancellationToken cancellationToken)
        {
            AccountSaving account = await _context.Set<AccountSaving>().FindAsync(new object[] { accountId }, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {accountId} not found");

            account.AccountStatus = AccountStatus.Closed;

            await _context.SaveChangesAsync(cancellationToken);
            return account;
        }

        public async Task<AccountSaving?> DeleteAsync(Guid accountId, CancellationToken cancellationToken)
        {
            AccountSaving account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {accountId} not found");

            account.AccountStatus = AccountStatus.Closed;
            _context.Set<AccountSaving>().Remove(account);
            await _context.SaveChangesAsync(cancellationToken);

            return account;
        }

        public async Task<List<AccountSaving>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<AccountSaving>().Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
        }
        public async Task<Paginated<AccountSaving>> GetPaginatedAccountsAsync(IQueryable<AccountSaving> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<AccountSaving>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Transaction>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<AccountSaving?> GetbyIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            AccountSaving account = await _context.Set<AccountSaving>().FindAsync(new object[] { accountId }, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {accountId} not found");
            return account;
        }

        public async Task<List<Transaction>?> GetTransactionsHistory(Guid accountId, CancellationToken cancellationToken)
        {
            List<Transaction> transactions = await _context.Set<Transaction>()
           .Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProductId == accountId) ||
           (t is Transfer && (((Transfer)t).SourceProductId == accountId || ((Transfer)t).DestinationProductId == accountId)) ||
           (t is Withdraw && ((Withdraw)t).SourceProductId == accountId)
           )
        .OrderByDescending(t => t.TransactionDate)
        .ToListAsync(cancellationToken);

            if (transactions == null || transactions.Count == 0)
            {
                throw new InvalidOperationException($"No transactions found for the account with ID {accountId}.");
            }

            return transactions;
        }

        public async Task<AccountSaving?> UpdateAsync(Guid accountId, AccountSaving entity, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {accountId} not found");
            if (entity.AccountName != "string")
            {
                account.AccountName = entity.AccountName;
            }

            account.AccountStatus = entity.AccountStatus != 0 ? entity.AccountStatus : account.AccountStatus;
            await CalculateAndResetInterest(account.Id, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return account;
        }

        public async Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == accountNumber, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {accountNumber} not found");
            ;
            return account;
        }

        public async Task CalculateAndResetInterest(Guid accountId, CancellationToken cancellationToken)
        {
            if (await HasReachedEndOfMonth(accountId))
            {
                AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account with the Id {accountId} not found");
                ;
                CalculateInterest(accountId);
                account.MonthlyInterestGenerated = 0;
            }
        }

        private async void CalculateInterest(Guid accountId)
        {
            AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId);

            if (account == null) return;

            decimal monthlyInterestRate = account.AnnualInterestRate / 12;

            account.MonthlyInterestGenerated = account.CurrentBalance * monthlyInterestRate;
            account.AnnualInterestProjected += account.MonthlyInterestGenerated * 12;
        }
        private async ValueTask<bool> HasReachedEndOfMonth(Guid accountId)
        {
            AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId);

            DateTime currentDate = DateTime.Today;
            return currentDate.Month != account?.DateOpened.Month || currentDate.Year != account.DateOpened.Year;
        }

        private static long GenerateBankAccountNumber()
        {
            Random random = new();

            long minAccountNumber = 1000000000;
            long maxAccountNumber = 9999999999;

            return (long)(random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);
        }

        private static int GenerateConfirmationNumber()
        {
            Random random = new();

            int minConfirmationNumber = 1000000;
            int maxConfirmationNumber = 9999999;

            int confirmationNumber = random.Next(minConfirmationNumber, maxConfirmationNumber + 1);

            return confirmationNumber;
        }
        private static long GenerateVoucherNumber()
        {
            Random random = new();

            long minConfirmationNumber = 1000000000;
            long maxConfirmationNumber = 9999999999;

            long confirmationNumber = (long)(random.NextDouble() * (maxConfirmationNumber - minConfirmationNumber) + minConfirmationNumber);

            return confirmationNumber;
        }

        public IQueryable<AccountSaving> GetAllQueryable()
        {
            return _context.Set<AccountSaving>();
        }
  
        public IQueryable<Transaction> GetTransactionsByAccountQueryable(Guid accountId)
        {
            return _context.Transactions.Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProductId == accountId) ||
            (t is Transfer && (((Transfer)t).SourceProductId == accountId ||
            ((Transfer)t).DestinationProductId == accountId)) ||
            (t is Withdraw && ((Withdraw)t).SourceProductId == accountId)
           )
        .OrderByDescending(t => t.TransactionDate);
        }

    }
}
