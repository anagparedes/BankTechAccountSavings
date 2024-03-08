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

        public async Task<AccountSaving> CreateAsync(AccountSaving entity)
        {
            entity.AccountNumber = GenerateBankAccountNumber();

            while (await _context.Set<AccountSaving>().AnyAsync(e => e.AccountNumber == entity.AccountNumber))
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

            _context.Set<AccountSaving>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Transfer?> AddDepositAsync(int amount, Guid accountId, string description, TransactionType transactionType)
        {
            var account = await _context.AccountSavings.FindAsync(accountId);
            if (account == null) return null;
            if (amount <= 0)
            {
                throw new ApplicationException("Invalid Deposit Amount");
            }
            account.CurrentBalance += amount;
            account.Credit = amount;

            Transfer newTransaction = new()
            {
                DestinationProduct = account,
                DestinationProductId = account.Id,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = description,
                TransactionType = transactionType,
                Amount = amount,
                Commission = 100,
                Tax = (0.0015 * amount),
                Total = (amount + 100 + (0.0015 * amount))
            };
            account.TransactionsAsDestination.Add(newTransaction);
            _context.Set<Transfer>().Add(newTransaction);

            Console.WriteLine(_context.Entry(account).State);

            await _context.SaveChangesAsync();
            return account.TransactionsAsDestination.LastOrDefault();
        }

        public async Task<Transfer?> WithDrawAsync(int amount, Guid accountId)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(accountId);
            if (account == null) return null;
            if (account.CurrentBalance < amount)
            {
                throw new ApplicationException("Insufficient Funds");
            }
            else if (amount <= 0)
            {
                throw new ApplicationException("Invalid withdraw amount");
            }
            account.CurrentBalance -= amount;
            account.Debit = amount;

            Transfer newTransaction = new()
            {
                DestinationProduct = account,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Amount = amount,

            };
            _context.Set<Transfer>().Add(newTransaction);
            account.TransactionsAsSource.Add(newTransaction);
            await _context.SaveChangesAsync();
            return account.TransactionsAsSource.LastOrDefault();
        }

        public async Task<Transfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, int transferAmount, TransactionType transactionType)
        {
            AccountSaving? fromAccount = await _context.Set<AccountSaving>().FindAsync(fromAccountId);
            AccountSaving? toAccount = await _context.Set<AccountSaving>().FindAsync(toAccountId);
            if (fromAccount == null || toAccount == null) return null;

            if (transferAmount <= 0)
            {
                throw new ApplicationException("transfer amount must be positive");
            }
            else if (transferAmount == 0)
            {
                throw new ApplicationException("invalid transfer amount");
            }

            if (fromAccount.CurrentBalance < transferAmount)
            {
                throw new ApplicationException("Insufficient Funds");
            }
            fromAccount.CurrentBalance -= transferAmount;
            fromAccount.Debit = transferAmount;
            toAccount.CurrentBalance += transferAmount;
            toAccount.Credit = transferAmount;
            Transfer newTransaction = new()
            {
                SourceProduct = fromAccount,
                DestinationProduct = toAccount,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                TransactionType = transactionType,
                TransactionStatus = TransactionStatus.Completed,
                Amount = transferAmount,
                Commission = 100,
                Tax = (0.0015 * transferAmount),
                Total = (transferAmount + 100 + (0.0015 * transferAmount))

            };
            fromAccount.TransactionsAsSource.Add(newTransaction);
            toAccount.TransactionsAsDestination.Add(newTransaction);
            return newTransaction;

        }

        public async Task<AccountSaving?> CloseAccountSavingAsync(Guid accountId)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(accountId);
            if (account == null) return null;

            account.AccountStatus = AccountStatus.Closed;

            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<AccountSaving?> DeleteAsync(Guid accountId)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId);
            if (account == null) return null;
            account.AccountStatus = AccountStatus.Closed;
            _context.Set<AccountSaving>().Remove(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<List<AccountSaving>> GetAllAsync()
        {
            return await _context.Set<AccountSaving>().ToListAsync();
        }

        public async Task<AccountSaving?> GetbyIdAsync(Guid accountId)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(accountId);
            if (account == null) return null;
            return account;
        }

        public async Task<List<Transfer>?> GetTransactionsHistory(Guid accountId)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(accountId);
            if (account == null) return null;

            List<Transfer> transactions = account.TransactionsAsSource
                .Concat(account.TransactionsAsDestination)
                .ToList();

            return transactions;
        }

        public async Task<AccountSaving?> UpdateAsync(Guid accountId, AccountSaving entity)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId);

            if (account == null)
                return null;
            if(entity.AccountName != "string")
            {
                account.AccountName = entity.AccountName;
            }

            account.AccountStatus = entity.AccountStatus != 0 ? entity.AccountStatus : account.AccountStatus;
            await CalculateAndResetInterest(account.Id);

            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == accountNumber);
            if (account == null) return null;
            return account;
        }

        public async Task CalculateAndResetInterest(Guid accountId)
        {
            if (await HasReachedEndOfMonth(accountId))
            {
                AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId);
                if (account == null) return;
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
    }
}
