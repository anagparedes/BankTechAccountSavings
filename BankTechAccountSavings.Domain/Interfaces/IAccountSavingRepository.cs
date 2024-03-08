using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface IAccountSavingRepository: IRepository<AccountSaving>
    {
        Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber);
        Task<List<Transfer>?> GetTransactionsHistory(Guid accountId);
        Task<Transfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, int transferAmount, TransactionType transactionType);
        Task<Transfer?> AddDepositAsync(int amount, Guid accountId, string description, TransactionType transactionType);
        Task<Transfer?> WithDrawAsync(int amount, Guid accountId);
        Task<AccountSaving?> CloseAccountSavingAsync(Guid accountId);


    }
}
