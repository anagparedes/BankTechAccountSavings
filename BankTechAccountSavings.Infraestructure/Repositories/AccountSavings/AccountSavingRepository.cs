using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BankTechAccountSavings.Infraestructure.Repositories.AccountSavings
{
    internal class AccountSavingRepository(AccountSavingDbContext context) : IAccountSavingRepository
    {
        private readonly AccountSavingDbContext _context = context;
        private readonly Random _random = new();
        public async Task<AccountSaving> CreateAsync(AccountSaving entity, CancellationToken cancellationToken)
        {
            entity.AccountNumber = GenerateBankAccountNumber();

            while (await _context.Set<AccountSaving>().AnyAsync(e => e.AccountNumber == entity.AccountNumber, cancellationToken))
            {
                entity.AccountNumber = GenerateBankAccountNumber();
            }

            entity.AccountName = $"CTA.{entity.AccountType}";
            entity.Bank = "BankTech";
            entity.CurrentBalance = entity.CurrentBalance;
            entity.AnnualInterestRate = 0.30m / 100;
            entity.MonthlyInterestGenerated = 0;
            entity.Currency = entity.Currency != 0 ? entity.Currency : Currency.DOP;
            entity.DateOpened = DateTime.UtcNow.Date.ToLocalTime();
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
                DestinationProductNumber = account.AccountNumber,
                Credit = amount,
                TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
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
                AccountName = account.AccountName,
                SourceProduct = account,
                SourceProductId = account.Id,
                SourceProductNumber = account.AccountNumber,
                WithdrawPassword = GenerateWithdrawPassword(),
                WithdrawCode = GenerateWithdrawCode(),
                Debit = amount,
                Amount = amount,
                TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = $"Withdraw on day: {DateTime.UtcNow.Date.ToLocalTime()}",
                TransactionType = TransactionType.WithDraw,
                TransactionStatus = TransactionStatus.Completed,
                Tax = (decimal)(0.0015 * amount),
                Total = (decimal)(amount + 100 + (0.0015 * amount))
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
                SourceProductNumber = fromAccount.AccountNumber,
                DestinationProduct = toAccount,
                DestinationProductId = toAccount.Id,
                DestinationProductNumber = toAccount.AccountNumber,
                TransferType = transferType,
                TransactionType = TransactionType.Transfer,
                Description = description,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                TransactionStatus = TransactionStatus.Completed,
                Amount = transferAmount,
                Commission = commission,
                Tax = (decimal)(0.0015 * transferAmount),
                Total = (decimal)(transferAmount + 100 + (0.0015 * transferAmount)),
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

        public async Task<AccountSaving?> DeleteAsync(Guid accountId, string reasonToCloseAccount, CancellationToken cancellationToken)
        {
            AccountSaving account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");

            account.AccountStatus = AccountStatus.Closed;
            _context.Set<AccountSaving>().Remove(account);
            await _context.SaveChangesAsync(cancellationToken);

            return account;
        }

        public async Task<List<AccountSaving>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<AccountSaving>().Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
        }
        public async Task<Paginated<AccountSaving>> GetAccountsPaginatedAsync(IQueryable<AccountSaving> queryable, int page, int pageSize)
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
            AccountSaving account = await _context.Set<AccountSaving>().FindAsync(new object[] { accountId }, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");
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
                throw new InvalidOperationException($"No transactions found for the account");
            }

            return transactions;
        }

        public async Task<AccountSaving?> UpdateAsync(Guid accountId, AccountSaving entity, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");
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
                AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");
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

        private long GenerateBankAccountNumber()
        {
            long minAccountNumber = 1000000000;
            long maxAccountNumber = 9999999999;
            long accountNumber = (long)(_random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);

            return accountNumber;
        }

        private long GenerateWithdrawPassword()
        {
            long minAccountNumber = 10000000;
            long maxAccountNumber = 99999999;
            long accountNumber = (long)(_random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);
            return accountNumber;
        }

        private long GenerateWithdrawCode()
        {
            long minAccountNumber = 1000;
            long maxAccountNumber = 9999;

            long accountNumber = (long)(_random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);
            return accountNumber;
        }

        private int GenerateConfirmationNumber()
        {

            int minConfirmationNumber = 1000000;
            int maxConfirmationNumber = 9999999;

            int confirmationNumber = _random.Next(minConfirmationNumber, maxConfirmationNumber + 1);

            return confirmationNumber;
        }
        private long GenerateVoucherNumber()
        {
            long minConfirmationNumber = 1000000000;
            long maxConfirmationNumber = 9999999999;

            long confirmationNumber = (long)(_random.NextDouble() * (maxConfirmationNumber - minConfirmationNumber) + minConfirmationNumber);

            return confirmationNumber;
        }

        public IQueryable<AccountSaving> GetAllQueryable(int clientId)
        {
            return _context.Set<AccountSaving>().Where(acc => acc.ClientId == clientId && !acc.IsDeleted);
        }

        public IQueryable<Transaction> GetTransactionsByAccountQueryable(int clientId)
        {
            return _context.Transactions.Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProduct!.ClientId == clientId) ||
            (t is Transfer && (((Transfer)t).SourceProduct!.ClientId == clientId ||
            ((Transfer)t).DestinationProduct!.ClientId == clientId)) ||
            (t is Withdraw && ((Withdraw)t).SourceProduct!.ClientId == clientId)
            )
                .OrderByDescending(t => t.TransactionDate);
        }

        public IQueryable<Transaction> GetTransactionsByAccountNumberQueryable(long accountNumber)
        {
            return _context.Transactions.Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProductNumber == accountNumber) ||
            (t is Transfer && (((Transfer)t).SourceProductNumber == accountNumber ||
            ((Transfer)t).DestinationProductNumber == accountNumber)) ||
            (t is Withdraw && ((Withdraw)t).SourceProductNumber == accountNumber)
           )
        .OrderByDescending(t => t.TransactionDate);
        }

        public async Task<Transfer?> TransferFundsByAccountNumberAsync(long fromAccountNumber, long toAccountNumber, string description, int transferAmount, TransferType transferType, CancellationToken cancellationToken = default)
        {
            AccountSaving? fromAccount = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == fromAccountNumber, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {fromAccountNumber} not found");
            AccountSaving? toAccount = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == toAccountNumber, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {toAccountNumber} not found");

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
                SourceProductNumber = fromAccount.AccountNumber,
                DestinationProduct = toAccount,
                DestinationProductId = toAccount.Id,
                DestinationProductNumber = toAccount.AccountNumber,
                TransferType = transferType,
                TransactionType = TransactionType.Transfer,
                Description = description,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                TransactionStatus = TransactionStatus.Completed,
                Amount = transferAmount,
                Commission = commission,
                Tax = (decimal)(0.0015 * transferAmount),
                Total = (decimal)(transferAmount + 100 + (0.0015 * transferAmount)),
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

        public async Task<Deposit?> AddDepositByAccountNumberAsync(int amount, long accountNumber, string description, CancellationToken cancellationToken = default)
        {
            var account = await _context.AccountSavings.FirstOrDefaultAsync(s => s.AccountNumber == accountNumber, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {accountNumber} not found");
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
                DestinationProductNumber = account.AccountNumber,
                Credit = amount,
                TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
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

        public async Task<Withdraw?> WithDrawByAccountNumberAsync(int amount, long accountNumber, CancellationToken cancellationToken = default)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == accountNumber, cancellationToken) ?? throw new InvalidOperationException($"The Account with the number {accountNumber} not found");
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
                AccountName = account.AccountName,
                SourceProduct = account,
                SourceProductId = account.Id,
                SourceProductNumber = account.AccountNumber,
                WithdrawPassword = GenerateWithdrawPassword(),
                WithdrawCode = GenerateWithdrawCode(),
                Debit = amount,
                Amount = amount,
                TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = $"Withdraw on day: {DateTime.UtcNow.Date.ToLocalTime()}",
                TransactionType = TransactionType.WithDraw,
                TransactionStatus = TransactionStatus.Completed,
                Tax = (decimal)(0.0015 * amount),
                Total = (decimal)(amount + 100 + (0.0015 * amount))
            };
            account.CurrentBalance -= (decimal)newWithdraw.Total;
            await _context.Set<Withdraw>().AddAsync(newWithdraw, cancellationToken);
            account.WithDraws.Add(newWithdraw);
            await _context.SaveChangesAsync(cancellationToken);
            return account.WithDraws.LastOrDefault();
        }

        public async Task<List<Transaction>?> GetTransactionsHistoryByAccountNumber(long accountNumber, CancellationToken cancellationToken = default)
        {
            List<Transaction> transactions = await _context.Set<Transaction>()
           .Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProductNumber == accountNumber) ||
           (t is Transfer && (((Transfer)t).SourceProductNumber == accountNumber || ((Transfer)t).DestinationProductNumber == accountNumber)) ||
           (t is Withdraw && ((Withdraw)t).SourceProductNumber == accountNumber)
           )
        .OrderByDescending(t => t.TransactionDate)
        .ToListAsync(cancellationToken);

            if (transactions == null || transactions.Count == 0)
            {
                throw new InvalidOperationException($"No transactions found for the account with number {accountNumber}.");
            }

            return transactions;
        }
    }
}
