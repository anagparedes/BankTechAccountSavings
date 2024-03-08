using Azure.Identity;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BankTechAccountSavings.Infraestructure.Repositories.AccountSavings
{
    public class AccountSavingRepository(AccountSavingDbContext context): IAccountSavingRepository
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
           /* entity.AvailableBalance = entity.CurrentBalance;
            entity.AverageBalance = entity.CurrentBalance;*/
            entity.AnnualInterestRate = 0.30m;
            entity.MonthlyInterestRate = (entity.AnnualInterestRate / 12);
            entity.Currency = entity.Currency != 0 ? entity.Currency : Domain.Enums.Currency.DOP;
            entity.DateOpened = DateTime.Today;
            entity.AccountStatus = Domain.Enums.AccountStatus.Active;

            _context.Set<AccountSaving>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<AccountSaving?> AddDepositAsync(int amount, Guid id, string description, TransactionType transactionType)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(id);
            if (account == null) return null;
            if (amount <= 0)
            {
                throw new ApplicationException("invalid deposit amount");
            }
            account.CurrentBalance += amount;
            account.Credit = amount;

            Transaction newTransaction = new()
            {
                DestinationProduct = account,
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
            _context.Set<Transaction>().Add(newTransaction);
            account.TransactionHistory.Add(newTransaction);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<AccountSaving?> WithDrawAsync(int amount, Guid id)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(id);
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

            Transaction newTransaction = new()
            {
                DestinationProduct = account,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Amount = amount,

            };
            _context.Set<Transaction>().Add(newTransaction);
            account.TransactionHistory.Add(newTransaction);
            await _context.SaveChangesAsync();
            return account;
        }
       
        public async Task<AccountSaving?> CloseAccountSavingAsync(Guid id)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(id);
            if (account == null) return null;

            account.AccountStatus = AccountStatus.Closed;

            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<AccountSaving?> DeleteAsync(Guid id)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == id);
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

        public async Task<AccountSaving?> GetbyIdAsync(Guid id)
        {
            var account = await _context.Set<AccountSaving>().FindAsync(id);
            if (account == null) return null;
            return account;
        }

        public async Task<AccountSaving?> UpdateAsync(Guid id, AccountSaving entity)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == id);

            if (account == null)
                return null;
            entity.AccountName ??= account.AccountName;
            account.AccountStatus = entity.AccountStatus != 0 ? entity.AccountStatus: account.AccountStatus;
            /*TODO: Implement an automatic method to update CurrentBalance and AverageBalance */
         /*   account.CurrentBalance = entity.CurrentBalance != 0 ? entity.CurrentBalance: account.CurrentBalance;
            account.AverageBalance = entity.AverageBalance != 0 ? entity.AverageBalance: account.AverageBalance;*/

            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == accountNumber);
            if (account == null) return null;
            return account;
        }

        private async Task<decimal?> CalculateMonthlyInterest(Guid id)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == id);

            if (account == null)
                return null;

            decimal monthlyInterest = account.CurrentBalance * account.MonthlyInterestRate;

            return monthlyInterest;
        }
        private async Task<decimal?> CalculateAnnualInterest(Guid id)
        {

            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == id);

            if (account == null)
                return null;
         
            decimal annualInterest = account.CurrentBalance * account.AnnualInterestRate;

            return annualInterest;
        }

        private static long GenerateBankAccountNumber()
        {
            Random random = new();

            long minAccountNumber = 10000000000;
            long maxAccountNumber = 99999999999;

            long accountNumber = (long)(random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);

            return accountNumber;
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
