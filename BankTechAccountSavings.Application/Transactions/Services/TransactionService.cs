using AutoMapper;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Application.Transactions.Interfaces;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;

namespace BankTechAccountSavings.Application.Transactions.Services
{
    internal class TransactionService(ITransactionRepository transactionRepository, IMapper mapper) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GetTransaction>> GetTransactions()
        {
            List<Transaction> transactions = await _transactionRepository.GetAllTransactions();
            return transactions.Select(tr => _mapper.Map<GetTransaction>(tr)).ToList();
        }

        public async Task<Paginated<GetTransaction>> GetPaginatedTransactionsAsync(int page, int pageSize)
        {
            IQueryable<Transaction> queryable = _transactionRepository.GetAllTransactionQueryable();
            Paginated<Transaction> paginatedResult = await _transactionRepository.GetTransactionsPaginatedAsync(queryable, page, pageSize);

            List<GetTransaction> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetTransaction>(st)).ToList() :
                [];

            return new Paginated<GetTransaction>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<List<GetTransfer>?> GetAllTransfers()
        {
            List<Transfer> transactions = await _transactionRepository.GetAllTransfersAsync();
            return transactions.Select(tr => _mapper.Map<GetTransfer>(tr)).ToList();
        }

        public async Task<Paginated<GetTransfer>> GetPaginatedTransfersAsync(int page, int pageSize)
        {
            IQueryable<Transfer> queryable = _transactionRepository.GetAllTransferQueryable();
            Paginated<Transfer> paginatedResult = await _transactionRepository.GetTransfersPaginatedAsync(queryable, page, pageSize);

            List<GetTransfer> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetTransfer>(st)).ToList() :
                [];

            return new Paginated<GetTransfer>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<List<GetTransfer>?> GetAllTransfersByAccount(Guid accountId)
        {
            List<Transfer> transactions = await _transactionRepository.GetAllTransfersByAccountAsync(accountId);
            return transactions.Select(tr => _mapper.Map<GetTransfer>(tr)).ToList();
        }

        public async Task<GetTransfer?> GetTransferbyId(Guid id)
        {
            Transfer? transaction = await _transactionRepository.GetTransferbyIdAsync(id);
            return _mapper.Map<GetTransfer>(transaction);
        }

        public async Task<List<GetDeposit>?> GetAllDeposits()
        {
            List<Deposit> transactions = await _transactionRepository.GetAllDepositsAsync();
            return transactions.Select(tr => _mapper.Map<GetDeposit>(tr)).ToList();
        }

        public async Task<Paginated<GetDeposit>> GetPaginatedDepositsAsync(int page, int pageSize)
        {
            IQueryable<Deposit> queryable = _transactionRepository.GetAllDepositQueryable();
            Paginated<Deposit> paginatedResult = await _transactionRepository.GetDepositsPaginatedAsync(queryable, page, pageSize);

            List<GetDeposit> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetDeposit>(st)).ToList() :
                [];

            return new Paginated<GetDeposit>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<List<GetDeposit>?> GetAllDepositsByAccount(Guid accountId)
        {
            List<Deposit> transactions = await _transactionRepository.GetAllDepositsByAccountAsync(accountId);
            return transactions.Select(tr => _mapper.Map<GetDeposit>(tr)).ToList();
        }

        public async Task<GetDeposit?> GetDepositbyId(Guid id)
        {
            Deposit? transaction = await _transactionRepository.GetDepositbyIdAsync(id);
            return _mapper.Map<GetDeposit>(transaction);
        }

        public async Task<List<GetWithdraw>?> GetAllWithdraws()
        {
            List<Withdraw> transactions = await _transactionRepository.GetAllWithdrawsAsync();
            return transactions.Select(tr => _mapper.Map<GetWithdraw>(tr)).ToList();
        }

        public async Task<Paginated<GetWithdraw>> GetPaginatedWithdrawsAsync(int page, int pageSize)
        {
            IQueryable<Withdraw> queryable = _transactionRepository.GetAllWithdrawQueryable();
            Paginated<Withdraw> paginatedResult = await _transactionRepository.GetWithdrawsPaginatedAsync(queryable, page, pageSize);

            List<GetWithdraw> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetWithdraw>(st)).ToList() :
                [];

            return new Paginated<GetWithdraw>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<List<GetWithdraw>?> GetAllWithdrawsByAccount(Guid accountId)
        {
            List<Withdraw> transactions = await _transactionRepository.GetAllWithdrawsByAccountAsync(accountId);
            return transactions.Select(tr => _mapper.Map<GetWithdraw>(tr)).ToList();
        }

        public async Task<GetWithdraw?> GetWithdrawbyId(Guid id)
        {
            Withdraw? transaction = await _transactionRepository.GetWithdrawbyIdAsync(id);
            return _mapper.Map<GetWithdraw>(transaction);
        }

        public async Task<Paginated<GetDeposit>> GetPaginatedDepositsByAccountAsync(Guid accountId, int page, int pageSize)
        {
            IQueryable<Deposit> queryable = _transactionRepository.GetDepositsByAccountQueryable(accountId);

            Paginated<Deposit> paginatedResult = await _transactionRepository.GetDepositsPaginatedAsync(queryable, page, pageSize);

            List<GetDeposit> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetDeposit>(st)).ToList() :
                [];

            return new Paginated<GetDeposit>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<GetWithdraw>> GetPaginatedWithdrawsByAccountAsync(Guid accountId, int page, int pageSize)
        {
            IQueryable<Withdraw> queryable = _transactionRepository.GetWithdrawsByAccountQueryable(accountId);

            Paginated<Withdraw> paginatedResult = await _transactionRepository.GetWithdrawsPaginatedAsync(queryable, page, pageSize);

            List<GetWithdraw> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetWithdraw>(st)).ToList() :
                [];

            return new Paginated<GetWithdraw>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<GetTransfer>> GetPaginatedTransfersByAccountAsync(Guid accountId, int page, int pageSize)
        {
            IQueryable<Transfer> queryable = _transactionRepository.GetTransfersByAccountQueryable(accountId);

            Paginated<Transfer> paginatedResult = await _transactionRepository.GetTransfersPaginatedAsync(queryable, page, pageSize);

            List<GetTransfer> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetTransfer>(st)).ToList() :
                [];

            return new Paginated<GetTransfer>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public string FormatErrorResponse(string errorMessage)
        {
            return $"\"errorMessage\": \"{errorMessage}\"";
        }
    }
}
