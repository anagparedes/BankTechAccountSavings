using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankTechAccountSavings.Infraestructure.Repositories.Transactions
{
    public class TransactionRepository(AccountSavingDbContext context): ITransactionRepository
    {
        private readonly AccountSavingDbContext _context = context;

        public async Task<List<Transfer>> GetAllAsync()
        {
            return await _context.Set<Transfer>().ToListAsync();
        }

        public async Task<Transfer?> GetbyIdAsync(Guid transactionId)
        {
            var transaction = await _context.Set<Transfer>().FindAsync(transactionId);
            if (transaction == null) return null;
            return transaction;
        }
    }
}
