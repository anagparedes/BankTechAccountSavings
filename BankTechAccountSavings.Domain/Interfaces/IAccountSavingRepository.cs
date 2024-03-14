using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Interfaces
{
    internal interface IAccountSavingRepository: IRepository<AccountSaving>
    {
        internal Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber, CancellationToken cancellationToken = default);
        internal Task<List<Transaction>?> GetTransactionsHistory(Guid accountId, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> GetTransactionsHistoryByAccountNumber(long accountNumber, CancellationToken cancellationToken = default);
        Task<Deposit?> CreateDepositAsync(Deposit entity, CancellationToken cancellationToken = default);
        Task<Transfer?> CreateTransferAsync(Transfer entity, CancellationToken cancellationToken = default);
        Task<Withdraw?> CreateWithdrawAsync(Withdraw entity, CancellationToken cancellationToken = default);
        //internal Task<Transfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transferType, CancellationToken cancellationToken = default);
        //internal Task<Deposit?> AddDepositAsync(int amount, Guid accountId, string description, CancellationToken cancellationToken = default);
        //internal Task<Withdraw?> WithDrawAsync(int amount, Guid accountId, CancellationToken cancellationToken = default);
        //internal Task<Transfer?> TransferFundsByAccountNumberAsync(long fromAccountNumber, long toAccountNumber, string description, int transferAmount, TransferType transferType, CancellationToken cancellationToken = default);
        //internal Task<Deposit?> AddDepositByAccountNumberAsync(int amount, long accountNumber, string description, CancellationToken cancellationToken = default);
        //internal Task<Withdraw?> WithDrawByAccountNumberAsync(int amount, long accountNumber, CancellationToken cancellationToken = default);
        internal IQueryable<AccountSaving> GetAllQueryable(int clientId);
        internal IQueryable<Transaction> GetTransactionsByAccountQueryable(int clientId);
        internal IQueryable<Transaction> GetTransactionsByAccountNumberQueryable(long accountNumber);
        internal IQueryable<Transfer> GetTransfersByAccountQueryable(int ClientId);
        internal Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize);
        internal Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize);
    }
}
