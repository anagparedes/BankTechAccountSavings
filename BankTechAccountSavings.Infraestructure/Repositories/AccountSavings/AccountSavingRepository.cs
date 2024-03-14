using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
            return entity;
        }

        public async Task<AccountSaving?> DeleteAsync(Guid accountId, string reasonToCloseAccount, CancellationToken cancellationToken)
        {
            AccountSaving account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");

            account.AccountStatus = AccountStatus.Closed;
            _context.Set<AccountSaving>().Remove(account);

            return account;
        }

        public async Task<List<AccountSaving>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<AccountSaving>().Where(x => !x.IsDeleted).ToListAsync(cancellationToken) ?? throw new InvalidOperationException("Invalid Deposit Amount");
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

        public async Task<AccountSaving?> UpdateAsync(Guid accountId, AccountSaving entity, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");
            if (entity.AccountName != "string")
            {
                account.AccountName = entity.AccountName;
            }

            account.AccountStatus = entity.AccountStatus != 0 ? entity.AccountStatus : account.AccountStatus;
            await CalculateAndResetInterest(account.Id, cancellationToken);

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
            IQueryable<AccountSaving> accounts = _context.Set<AccountSaving>().Where(acc => acc.ClientId == clientId && !acc.IsDeleted)
                .OrderByDescending(t => t.CreatedDate);
            if (accounts.Any() == false)
            {
                throw new InvalidOperationException("The client doesn't has accounts");
            }
            return accounts;
        }

        public IQueryable<Transaction> GetTransactionsByAccountQueryable(int clientId)
        {
            IQueryable<Transaction> transactions = _context.Transactions.Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProduct!.ClientId == clientId) ||
            (t is Transfer && (((Transfer)t).SourceProduct!.ClientId == clientId ||
            ((Transfer)t).DestinationProduct!.ClientId == clientId)) ||
            (t is Withdraw && ((Withdraw)t).SourceProduct!.ClientId == clientId)
            )
                .OrderByDescending(t => t.CreatedDate);
            return transactions;
        }

        public IQueryable<Transaction> GetTransactionsByAccountNumberQueryable(long accountNumber)
        {
            IQueryable<Transaction> transactions = _context.Transactions.Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProductNumber == accountNumber) ||
            (t is Transfer && (((Transfer)t).SourceProductNumber == accountNumber ||
            ((Transfer)t).DestinationProductNumber == accountNumber)) ||
            (t is Withdraw && ((Withdraw)t).SourceProductNumber == accountNumber)
           )
        .OrderByDescending(t => t.CreatedDate);

            return transactions;
        }

        public async Task<Deposit?> CreateDepositAsync(Deposit entity, CancellationToken cancellationToken = default)
        {
            var account = await _context.AccountSavings.FirstOrDefaultAsync(s => s.Id == entity.DestinationProductId, cancellationToken) ?? throw new InvalidOperationException($"The Account don't exist");
            if (entity.Amount <= 0)
            {
                throw new InvalidOperationException("Invalid Deposit Amount");
            }
            account.CurrentBalance += entity.Amount;

            Deposit newDeposit = new()
            {
                DestinationProduct = account,
                DestinationProductId = account.Id,
                DestinationProductNumber = account.AccountNumber,
                TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = entity.Description,
                TransactionType = TransactionType.Deposit,
                TransactionStatus = TransactionStatus.Completed,
                Amount = entity.Amount,
            };
            account.Deposits.Add(newDeposit);
            await _context.Set<Deposit>().AddAsync(newDeposit, cancellationToken);

            return account.Deposits.LastOrDefault();
        }

        async Task<Transfer?> IAccountSavingRepository.CreateTransferAsync(Transfer entity, CancellationToken cancellationToken)
        {
            AccountSaving? fromAccount = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == entity.SourceProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Source Account was not found. Please ensure that the source account exists and is correctly specified.\r\n");
            AccountSaving? toAccount = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == entity.DestinationProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Destination Account not found. Please ensure that the destination account exists and is correctly specified.\r\n");

            if (entity.Amount <= 0 || entity.Amount > fromAccount.CurrentBalance)
            {
                throw new InvalidOperationException("Invalid transfer amount or insufficient funds");
            }

            int commission = (entity.TransferType == TransferType.LBTR) ? 100 : 0;

            Transfer newTransfer = new()
            {
                SourceProduct = fromAccount,
                SourceProductId = fromAccount.Id,
                SourceProductNumber = fromAccount.AccountNumber,
                DestinationProduct = toAccount,
                DestinationProductId = toAccount.Id,
                DestinationProductNumber = toAccount.AccountNumber,
                TransferType = entity.TransferType,
                TransactionType = TransactionType.Transfer,
                Description = entity.Description,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                TransactionStatus = TransactionStatus.Completed,
                Amount = entity.Amount,
                Commission = commission,
                Tax = 0.0015m * entity.Amount,
                Total = (entity.Amount + commission + (0.0015m * entity.Amount)),
                Credit = entity.Amount,
                Debit = entity.Amount,
            };

            fromAccount.CurrentBalance -= newTransfer.Total;
            toAccount.CurrentBalance += entity.Amount;

            fromAccount.TransfersAsSource.Add(newTransfer);
            toAccount.TransfersAsDestination.Add(newTransfer);

            return newTransfer;
        }

        async Task<Withdraw?> IAccountSavingRepository.CreateWithdrawAsync(Withdraw entity, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == entity.SourceProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Account not found. Please ensure that the account exists and is correctly specified.\r\n");
            if (account.CurrentBalance < entity.Amount)
            {
                throw new InvalidOperationException("Insufficient Funds");
            }
            else if (entity.Amount <= 0)
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
                Debit = entity.Amount,
                Amount = entity.Amount,
                TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = $"Withdraw on day: {DateTime.UtcNow.Date.ToLocalTime()}",
                TransactionType = TransactionType.WithDraw,
                TransactionStatus = TransactionStatus.Completed,
                Tax = 0.0015m * entity.Amount,
                Total = (entity.Amount + (0.0015m * entity.Amount))
            };
            account.CurrentBalance -= newWithdraw.Total;
            await _context.Set<Withdraw>().AddAsync(newWithdraw, cancellationToken);
            account.WithDraws.Add(newWithdraw);
            return account.WithDraws.LastOrDefault();
        }

        public IQueryable<Transfer> GetTransfersByAccountQueryable(int clientId)
        {
            return _context.Transfers.Where(w => w.SourceProduct!.ClientId == clientId || w.DestinationProduct!.ClientId == clientId)
                .OrderByDescending(t => t.TransactionDate);
        }

        public async Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Transfer>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
