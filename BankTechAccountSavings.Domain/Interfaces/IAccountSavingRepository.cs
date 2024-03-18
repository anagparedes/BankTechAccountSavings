using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Domain.Interfaces
{
    internal interface IAccountSavingRepository: IRepository<AccountSaving>
    {
        Task<Deposit?> CreateDepositAsync(Deposit entity, CancellationToken cancellationToken = default);
        Task<Transfer?> CreateBankTransferAsync(Transfer entity, CancellationToken cancellationToken = default);
        Task<Transfer?> CreateInterBankTransferAsync(Transfer entity, CancellationToken cancellationToken = default);
        Task<Withdraw?> CreateWithdrawAsync(Withdraw entity, CancellationToken cancellationToken = default);
  
        internal IQueryable<AccountSaving> GetAllQueryable(int clientId);
        internal IQueryable<Transaction> GetTransactionsByClientQueryable(int clientId);
        internal IQueryable<Transaction> GetTransactionsByAccountNumberQueryable(long accountNumber);
        internal IQueryable<Transfer> GetTransfersByClientQueryable(int ClientId);
        internal IQueryable<Beneficiary> GetBeneficiariesByClientQueryable(int clientId);
        internal Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize);
        internal Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize);
        internal Task<Paginated<Beneficiary>> GetBeneficiaryPaginatedAsync(IQueryable<Beneficiary> queryable, int page, int pageSize);
    }
}
