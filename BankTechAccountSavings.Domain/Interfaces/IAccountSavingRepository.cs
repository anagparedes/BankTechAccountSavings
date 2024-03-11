using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Interfaces
{
    internal interface IAccountSavingRepository: IRepository<AccountSaving>
    {
        internal Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber, CancellationToken cancellationToken = default);
        internal Task<List<Transaction>?> GetTransactionsHistory(Guid accountId, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> GetTransactionsHistoryByAccountNumber(long accountNumber, CancellationToken cancellationToken = default);
        internal Task<Transfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transferType, CancellationToken cancellationToken = default);
        internal Task<Deposit?> AddDepositAsync(int amount, Guid accountId, string description, CancellationToken cancellationToken = default);
        internal Task<Withdraw?> WithDrawAsync(int amount, Guid accountId, CancellationToken cancellationToken = default);
        internal Task<Transfer?> TransferFundsByAccountNumberAsync(long fromAccountNumber, long toAccountNumber, string description, int transferAmount, TransferType transferType, CancellationToken cancellationToken = default);
        internal Task<Deposit?> AddDepositByAccountNumberAsync(int amount, long accountNumber, string description, CancellationToken cancellationToken = default);
        internal Task<Withdraw?> WithDrawByAccountNumberAsync(int amount, long accountNumber, CancellationToken cancellationToken = default);
        internal IQueryable<AccountSaving> GetAllQueryable();
        internal IQueryable<Transaction> GetTransactionsByAccountQueryable(Guid accountId);
        internal IQueryable<Transaction> GetTransactionsByAccountNumberQueryable(long accountNumber);
        internal Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize);
    }
}
