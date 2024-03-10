using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
namespace BankTechAccountSavings.Application.AccountSavings.Interfaces
{
    public interface IAccountSavingService
    {
        Task<List<GetAccountSaving>> GetAllAccountsAsync();
        internal Task<Paginated<GetAccountSaving>> GetPaginatedAccountsAsync(int page, int pageSize);
        internal Task<Paginated<GetTransaction>> GetPaginatedTransactionsByAccountAsync(Guid accountId, int page, int pageSize);
        Task<CreatedAccountSavingResponse?> CreateAccountSavingAsync(CreateAccountSaving createAccountSaving);
        Task<GetDeposit?> AddDepositAsync(int amount, Guid accountId, string description);
        Task<GetWithdraw?> WithDrawAsync(int amount, Guid accountId);
        Task<GetTransfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transactionType);
        Task<List<GetTransaction>?> GetTransactionsHistory(Guid accountId);
        Task<DeletedAccountSavingResponse?> CloseAccountSavingAsync(Guid accountId);
        Task<GetAccountSaving?> GetAccountSavingByIdAsync(Guid accountId);
        Task<GetAccountSaving?> GetAccountSavingByAccountNumberAsync(long accountNumber);
        Task<UpdatedAccountSavingResponse?> UpdateAccountSavingAsync(Guid accountId, UpdateAccountSaving updateAccountSaving);
        Task<DeletedAccountSavingResponse?> DeleteAccountSavingAsync(Guid accountId);
    }
}
