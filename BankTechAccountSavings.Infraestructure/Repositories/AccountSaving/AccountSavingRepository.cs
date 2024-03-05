using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;


namespace BankTechAccountSavings.Infraestructure.Repositories.AccountSaving
{
    public class AccountSavingRepository(AccountSavingDbContext context): IAccountSavingRepository
    {
        private readonly AccountSavingDbContext _context = context;

        public async Task<Domain.Entities.AccountSaving> AddAsync(Domain.Entities.AccountSaving newAccount)
        {
            newAccount.AccountStatus = Domain.Enums.AccountStatus.Active;
            newAccount.AccountId = GetNextAccountId();
            _context.AccountSavings.Add(newAccount);
            await _context.SaveChangesAsync();
            return newAccount;
        }

        public async Task<Domain.Entities.AccountSaving?> DeleteAsync(int id)
        {
            var account = await _context.AccountSavings.FirstOrDefaultAsync(p => p.AccountId == id);
            if (account == null)
                return null;

            _context.AccountSavings.Remove(account);
            await _context.SaveChangesAsync();

            return account;

        }

        public async Task<List<Domain.Entities.AccountSaving>> GetAllAsync()
        {
            return await _context.AccountSavings.ToListAsync();
        }

        public async Task<Domain.Entities.AccountSaving?> GetbyIdAsync(int id)
        {
            var account = await _context.AccountSavings.FirstOrDefaultAsync(p => p.AccountId == id);
            if (account == null)
                return null;
            return account;
        }

        public async Task<Domain.Entities.AccountSaving?> UpdateAsync(int id, Domain.Entities.AccountSaving newAccount)
        {
            var account = await _context.AccountSavings.FirstOrDefaultAsync(p => p.AccountId == id);

            if (account == null)
                return null;

            account.CurrentBalance = newAccount.CurrentBalance;
            account.AccountStatus = newAccount.AccountStatus;

            await _context.SaveChangesAsync();

            return account;
        }

        public int GetNextAccountId()
        {
            var maxProductId = _context.AccountSavings.Max(p => (int?)p.AccountId) ?? 0;
            return maxProductId + 1;
        }
    }
}
