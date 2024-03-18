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

        Task<Paginated<GetAccountSaving>> GetPaginatedAccountsByClientIdAsync(int clientId, int page, int pageSize);
        Task<Paginated<GetTransaction>> GetPaginatedTransactionsByClientIdAsync(int ClientId, int page, int pageSize);
        Task<Paginated<GetTransfer>> GetPaginatedTransfersByClientIdAsync(int clientId, int page, int pageSize);
        Task<Paginated<GetTransaction>> GetPaginatedTransactionsByAccountNumberAsync(long accountNumber, int page, int pageSize);
        Task<Paginated<GetBeneficiary>> GetPaginatedBeneficiariesByClientIdAsync(int clientId, int page, int pageSize);


        Task<GetDeposit?> AddDepositAsync(CreateDeposit entity);
        Task<GetTransfer?> CreateBankTransferAsync(CreateBankTransfer entity);
        Task<GetTransfer?> CreateInterBankTransferAsync(CreateInterBankTransfer entity);
        Task<GetWithdraw?> CreateWithdrawAsync(CreateWithdraw entity);
       
        string FormatErrorResponse(string errorMessage);
    }
}
