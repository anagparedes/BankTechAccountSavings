using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
namespace BankTechAccountSavings.Application.AccountSavings.Interfaces
{
    public interface IAccountSavingService
    {
        Task<List<GetAccountSaving>> GetAllAccountsAsync();
        Task<CreatedAccountSavingResponse?> CreateAccountSavingAsync(CreateAccountSaving createAccountSaving);
        Task<UpdatedAccountSavingResponse?> UpdateAccountSavingAsync(Guid accountId, UpdateAccountSaving updateAccountSaving);
        Task<DeletedAccountSavingResponse?> DeleteAccountSavingAsync(Guid accountId, string reasonToCloseAccount);

        Task<Paginated<GetAccountSaving>> GetPaginatedAccountsAsync(int clientId, int page, int pageSize);
        Task<Paginated<GetTransaction>> GetPaginatedTransactionsByAccountAsync(int ClientId, int page, int pageSize);
        Task<Paginated<GetTransfer>> GetPaginatedTransfersByAccountAsync(int clientId, int page, int pageSize);
        Task<Paginated<GetTransaction>> GetPaginatedTransactionsByAccountNumberAsync(long accountNumber, int page, int pageSize);

        Task<GetDeposit?> AddDepositAsync(CreateDeposit entity);
        Task<GetTransfer?> CreateTransferAsync(CreateTransfer entity);
        Task<GetWithdraw?> CreateWithdrawAsync(CreateWithdraw entity);
       
        string FormatErrorResponse(string errorMessage);
    }
}
