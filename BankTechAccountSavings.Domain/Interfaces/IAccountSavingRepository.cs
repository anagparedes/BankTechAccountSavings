using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Interfaces
{
    internal interface IAccountSavingRepository: IRepository<AccountSaving>
    {
        Task<Deposit?> CreateDepositAsync(Deposit entity, CancellationToken cancellationToken = default);
        Task<Transfer?> CreateTransferAsync(Transfer entity, CancellationToken cancellationToken = default);
        Task<Withdraw?> CreateWithdrawAsync(Withdraw entity, CancellationToken cancellationToken = default);
  
        internal IQueryable<AccountSaving> GetAllQueryable(int clientId);
        internal IQueryable<Transaction> GetTransactionsByAccountQueryable(int clientId);
        internal IQueryable<Transaction> GetTransactionsByAccountNumberQueryable(long accountNumber);
        internal IQueryable<Transfer> GetTransfersByAccountQueryable(int ClientId);
        internal Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize);
        internal Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize);
    }
}
