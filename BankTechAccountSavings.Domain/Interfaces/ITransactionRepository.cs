using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        internal Task<List<Transaction>> GetAllTransactions(CancellationToken cancellationToken = default);

        internal Task<List<Transfer>> GetAllTransfersAsync(CancellationToken cancellationToken = default);
        internal Task<List<Transfer>> GetAllTransfersByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
        internal Task<Transfer?> GetTransferbyIdAsync(Guid transactionId, CancellationToken cancellationToken = default);

        internal Task<List<Deposit>> GetAllDepositsAsync(CancellationToken cancellationToken = default);
        internal Task<List<Deposit>> GetAllDepositsByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
        internal Task<Deposit?> GetDepositbyIdAsync(Guid transactionId, CancellationToken cancellationToken = default);

        internal Task<List<Withdraw>> GetAllWithdrawsAsync(CancellationToken cancellationToken = default);
        internal Task<List<Withdraw>> GetAllWithdrawsByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
        internal Task<Withdraw?> GetWithdrawbyIdAsync(Guid transactionId, CancellationToken cancellationToken = default);

        internal IQueryable<Transaction> GetAllTransactionQueryable();
        internal IQueryable<Deposit> GetAllDepositQueryable();
        internal IQueryable<Transfer> GetAllTransferQueryable();
        internal IQueryable<Withdraw> GetAllWithdrawQueryable();

      
        internal IQueryable<Deposit> GetDepositsByAccountQueryable(Guid accountId);
        internal IQueryable<Withdraw> GetWithdrawsByAccountQueryable(Guid accountId);
        internal IQueryable<Transfer> GetTransfersByAccountQueryable(Guid accountId);

        internal Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize);
        internal Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize);
        internal Task<Paginated<Deposit>> GetDepositsPaginatedAsync(IQueryable<Deposit> queryable, int page, int pageSize);
        internal    Task<Paginated<Withdraw>> GetWithdrawsPaginatedAsync(IQueryable<Withdraw> queryable, int page, int pageSize);
    }
}
