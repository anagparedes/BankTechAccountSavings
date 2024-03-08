using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Infraestructure.Repositories.Transactions
{
    public class TransactionRepository(AccountSavingDbContext context): ITransactionRepository
    {
        private readonly AccountSavingDbContext _context = context;

        public async Task<Transaction?> DeleteAsync(Guid id)
        {
            var transaction = await _context.Set<Transaction>().FirstOrDefaultAsync(s => s.Id == id);
            if (transaction == null) return null;
            _context.Set<Transaction>().Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Set<Transaction>().ToListAsync();
        }

        public async Task<Transaction?> GetbyIdAsync(Guid id)
        {
            var transaction = await _context.Set<Transaction>().FindAsync(id);
            if (transaction == null) return null;
            return transaction;
        }

        public async Task<Transaction?> TransferFunds(Guid fromAccountId, Guid toAccountId, int transferAmount, TransactionType transactionType)
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
            Transaction newTransaction = new()
            {
                SourceProduct = fromAccount,
                DestinationProduct = toAccount,
                TransactionDate = DateTime.Today,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                TransactionType = transactionType,
                Amount = transferAmount,
                Commission = 100,
                Tax = (0.0015 * transferAmount),
                Total = (transferAmount + 100 + (0.0015 * transferAmount))

            };
            return newTransaction;

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
