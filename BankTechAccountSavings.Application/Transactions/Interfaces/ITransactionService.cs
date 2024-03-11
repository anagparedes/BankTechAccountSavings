using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Application.Transactions.Interfaces
{
    public interface ITransactionService
    {
        Task<List<GetTransaction>> GetTransactions();
        internal Task<Paginated<GetTransaction>> GetPaginatedTransactionsAsync(int page, int pageSize);

        Task<List<GetTransfer>?> GetAllTransfers();
        internal Task<Paginated<GetTransfer>> GetPaginatedTransfersAsync(int page, int pageSize);
        Task<List<GetTransfer>?> GetAllTransfersByAccount(Guid accountId);
        Task<GetTransfer?> GetTransferbyId(Guid transactionId);

        Task<List<GetDeposit>?> GetAllDeposits();
        internal Task<Paginated<GetDeposit>> GetPaginatedDepositsAsync(int page, int pageSize);
        Task<List<GetDeposit>?> GetAllDepositsByAccount(Guid accountId);
        Task<GetDeposit?> GetDepositbyId(Guid transactionId);

        Task<List<GetWithdraw>?> GetAllWithdraws();
        internal Task<Paginated<GetWithdraw>> GetPaginatedWithdrawsAsync(int page, int pageSize);
        Task<List<GetWithdraw>?> GetAllWithdrawsByAccount(Guid accountId);
        Task<GetWithdraw?> GetWithdrawbyId(Guid transactionId);

        
        internal Task<Paginated<GetDeposit>> GetPaginatedDepositsByAccountAsync(Guid accountId, int page, int pageSize);
        internal Task<Paginated<GetWithdraw>> GetPaginatedWithdrawsByAccountAsync(Guid accountId, int page, int pageSize);
        internal Task<Paginated<GetTransfer>> GetPaginatedTransfersByAccountAsync(Guid accountId, int page, int pageSize);

        string FormatErrorResponse(string errorMessage);
    }
}
