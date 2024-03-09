using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface IAccountSavingRepository: IRepository<AccountSaving>
    {
        Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> GetTransactionsHistory(Guid accountId, CancellationToken cancellationToken = default);
        Task<Transfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transactionType, CancellationToken cancellationToken = default);
        Task<Deposit?> AddDepositAsync(int amount, Guid accountId, string description, CancellationToken cancellationToken = default);
        Task<Withdraw?> WithDrawAsync(int amount, Guid accountId, CancellationToken cancellationToken = default);
        Task<AccountSaving?> CloseAccountSavingAsync(Guid accountId, CancellationToken cancellationToken = default);
        IQueryable<AccountSaving> GetAllQueryable();
        IQueryable<Transaction> GetTransactionsByAccountQueryable(Guid accountId);
        Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize);


    }
}
