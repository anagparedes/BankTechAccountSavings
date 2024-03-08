using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Enums;
namespace BankTechAccountSavings.Application.AccountSavings.Interfaces
{
    public interface IAccountSavingService
    {
        Task<List<GetAccountSaving>> GetAllAccountsAsync();
        Task<CreatedAccountSavingResponse?> CreateAccountSavingAsync(CreateAccountSaving createAccountSaving);
        Task<GetTransaction?> AddDepositAsync(int amount, Guid accountId, string description, TransactionType transactionType);
        Task<GetTransaction?> WithDrawAsync(int amount, Guid accountId);
        Task<GetTransaction?> TransferFunds(Guid fromAccountId, Guid toAccountId, int transferAmount, TransactionType transactionType);
        Task<List<GetTransaction>?> GetTransactionsHistory(Guid accountId);
        Task<DeletedAccountSavingResponse?> CloseAccountSavingAsync(Guid accountId);
        Task<GetAccountSaving?> GetAccountSavingByIdAsync(Guid accountId);
        Task<GetAccountSaving?> GetAccountSavingByAccountNumberAsync(long accountNumber);
        Task<UpdatedAccountSavingResponse?> UpdateAccountSavingAsync(Guid accountId, UpdateAccountSaving updateAccountSaving);
        Task<DeletedAccountSavingResponse?> DeleteAccountSavingAsync(Guid accountId);
    }
}
