using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankTechAccountSavings.Infraestructure.Repositories.Transactions
{
    internal class TransactionRepository(AccountSavingDbContext context) : ITransactionRepository
    {
        private readonly AccountSavingDbContext _context = context;

        public async Task<List<Transaction>> GetAllTransactions(CancellationToken cancellationToken)
        {
            List<Transaction> transactions = [];

            var deposits = await _context.Set<Deposit>()
                .Include(d => d.DestinationProduct)
                .ToListAsync(cancellationToken);

            transactions.AddRange(deposits);

            var transfers = await _context.Set<Transfer>()
                .Include(t => t.SourceProduct)
                .Include(t => t.DestinationProduct)
                .ToListAsync(cancellationToken);

            transactions.AddRange(transfers);

            var withdraws = await _context.Set<Withdraw>()
                .Include(w => w.SourceProduct)
                .ToListAsync(cancellationToken);

            transactions.AddRange(withdraws);

            transactions =
            [
                .. transactions
                                .OrderByDescending(t => t.TransactionDate)
,
            ];

            if (transactions == null || transactions.Count == 0)
            {
                throw new InvalidOperationException("No transactions found.");
            }

            return transactions;
        }

        public async Task<List<Transfer>> GetAllTransfersAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Transfer>().ToListAsync(cancellationToken);
        }

        public async Task<Transfer?> GetTransferbyIdAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var transaction = await _context.Set<Transfer>().FindAsync(new object[] { transactionId }, cancellationToken) ?? throw new InvalidOperationException("No transactions found.");
            return transaction;
        }

        public async Task<List<Transfer>> GetAllTransfersByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        {
            List<Transfer> transfers = await _context.Set<Transfer>()
                                                     .Include(t => t.SourceProduct)
                                                     .Include(t => t.DestinationProduct)
                                                     .Where(t => t.SourceProductId == accountId || t.DestinationProductId == accountId)
                                                     .OrderByDescending(t => t.TransactionDate)
                                                     .ToListAsync(cancellationToken);

            if (transfers == null || transfers.Count == 0)
            {
                throw new InvalidOperationException($"No transfers found for the account with ID {accountId}.");
            }

            return transfers;
        }

        public async Task<List<Deposit>> GetAllDepositsAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Deposit>().ToListAsync(cancellationToken);
        }

        public async Task<List<Deposit>> GetAllDepositsByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        {
            List<Deposit> deposits = await _context.Set<Deposit>()
                                                   .Include(a => a.DestinationProduct)
                                                   .Where(a => a.DestinationProductId == accountId)
                                                   .OrderByDescending(t => t.TransactionDate)
                                                   .ToListAsync(cancellationToken);

            if (deposits == null || deposits.Count == 0)
            {
                throw new InvalidOperationException($"No deposits found for the account with ID {accountId}.");
            }

            return deposits;
        }

        public async Task<Deposit?> GetDepositbyIdAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var transaction = await _context.Set<Deposit>().FindAsync(new object[] { transactionId }, cancellationToken) ?? throw new InvalidOperationException("No transactions found.");
            return transaction;
        }

        public async Task<List<Withdraw>> GetAllWithdrawsAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Withdraw>().ToListAsync(cancellationToken);
        }

        public async Task<List<Withdraw>> GetAllWithdrawsByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        {
            List<Withdraw> withdraws = await _context.Set<Withdraw>()
                                                     .Include(w => w.SourceProduct)
                                                     .Where(w => w.SourceProductId == accountId)
                                                     .OrderByDescending(w => w.TransactionDate)
                                                     .ToListAsync(cancellationToken);

            if (withdraws == null || withdraws.Count == 0)
            {
                throw new InvalidOperationException($"No withdrawals found for the account with ID {accountId}.");
            }

            return withdraws;
        }

        public async Task<Withdraw?> GetWithdrawbyIdAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var transaction = await _context.Set<Withdraw>().FindAsync(new object[] { transactionId }, cancellationToken) ?? throw new InvalidOperationException("No transactions found.");
            return transaction;
        }

        public IQueryable<Transaction> GetAllTransactionQueryable()
        {
            return _context.Set<Transaction>();
        }

        public IQueryable<Deposit> GetAllDepositQueryable()
        {
            return _context.Set<Deposit>();
        }

        public IQueryable<Transfer> GetAllTransferQueryable()
        {
            return _context.Set<Transfer>();
        }

        public IQueryable<Withdraw> GetAllWithdrawQueryable()
        {
            return _context.Set<Withdraw>();
        }

        public IQueryable<Deposit> GetDepositsByAccountQueryable(Guid accountId)
        {
            return _context.Deposits.Where(w => w.DestinationProductId == accountId);
        }

        public IQueryable<Withdraw> GetWithdrawsByAccountQueryable(Guid accountId)
        {
            return _context.Withdraws.Where(w => w.SourceProductId == accountId);
        }

        public IQueryable<Transfer> GetTransfersByAccountQueryable(Guid accountId)
        {
            return _context.Transfers.Where(w => w.SourceProductId == accountId || w.DestinationProductId == accountId);
        }

        public async Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Transaction>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Transfer>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<Deposit>> GetDepositsPaginatedAsync(IQueryable<Deposit> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Deposit>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<Withdraw>> GetWithdrawsPaginatedAsync(IQueryable<Withdraw> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Withdraw>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }
    }
}
