using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactions(CancellationToken cancellationToken = default);

        Task<List<Transfer>> GetAllTransfersAsync(CancellationToken cancellationToken = default);
        Task<List<Transfer>> GetAllTransfersByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<Transfer?> GetTransferbyIdAsync(Guid transactionId, CancellationToken cancellationToken = default);

        Task<List<Deposit>> GetAllDepositsAsync(CancellationToken cancellationToken = default);
        Task<List<Deposit>> GetAllDepositsByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<Deposit?> GetDepositbyIdAsync(Guid transactionId, CancellationToken cancellationToken = default);

        Task<List<Withdraw>> GetAllWithdrawsAsync(CancellationToken cancellationToken = default);
        Task<List<Withdraw>> GetAllWithdrawsByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<Withdraw?> GetWithdrawbyIdAsync(Guid transactionId, CancellationToken cancellationToken = default);

        IQueryable<Transaction> GetAllTransactionQueryable();
        IQueryable<Deposit> GetAllDepositQueryable();
        IQueryable<Transfer> GetAllTransferQueryable();
        IQueryable<Withdraw> GetAllWithdrawQueryable();

      
        IQueryable<Deposit> GetDepositsByAccountQueryable(Guid accountId);
        IQueryable<Withdraw> GetWithdrawsByAccountQueryable(Guid accountId);
        IQueryable<Transfer> GetTransfersByAccountQueryable(Guid accountId);

        Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize);
        Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize);
        Task<Paginated<Deposit>> GetDepositsPaginatedAsync(IQueryable<Deposit> queryable, int page, int pageSize);
        Task<Paginated<Withdraw>> GetWithdrawsPaginatedAsync(IQueryable<Withdraw> queryable, int page, int pageSize);
    }
}
