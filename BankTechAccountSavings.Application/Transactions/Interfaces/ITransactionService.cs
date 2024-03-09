using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Application.Transactions.Interfaces
{
    public interface ITransactionService
    {
        Task<List<GetTransaction>> GetTransactions();
        Task<Paginated<GetTransaction>> GetPaginatedTransactionsAsync(int page, int pageSize);

        Task<List<GetTransfer>?> GetAllTransfers();
        Task<Paginated<GetTransfer>> GetPaginatedTransfersAsync(int page, int pageSize);
        Task<List<GetTransfer>?> GetAllTransfersByAccount(Guid accountId);
        Task<GetTransfer?> GetTransferbyId(Guid transactionId);

        Task<List<GetDeposit>?> GetAllDeposits();
        Task<Paginated<GetDeposit>> GetPaginatedDepositsAsync(int page, int pageSize);
        Task<List<GetDeposit>?> GetAllDepositsByAccount(Guid accountId);
        Task<GetDeposit?> GetDepositbyId(Guid transactionId);

        Task<List<GetWithdraw>?> GetAllWithdraws();
        Task<Paginated<GetWithdraw>> GetPaginatedWithdrawsAsync(int page, int pageSize);
        Task<List<GetWithdraw>?> GetAllWithdrawsByAccount(Guid accountId);
        Task<GetWithdraw?> GetWithdrawbyId(Guid transactionId);

        
        Task<Paginated<GetDeposit>> GetPaginatedDepositsByAccountAsync(Guid accountId, int page, int pageSize);
        Task<Paginated<GetWithdraw>> GetPaginatedWithdrawsByAccountAsync(Guid accountId, int page, int pageSize);
        Task<Paginated<GetTransfer>> GetPaginatedTransfersByAccountAsync(Guid accountId, int page, int pageSize);
    }
}
