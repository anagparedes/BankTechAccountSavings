using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using AutoMapper;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Application.Transactions.Dtos;

namespace BankTechAccountSavings.Application.AccountSavings.Services
{
    public class AccountSavingService(IAccountSavingRepository accountSavingRepository, IMapper mapper) : IAccountSavingService

    {
        private readonly IAccountSavingRepository _accountSavingRepository = accountSavingRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<GetDeposit?> AddDepositAsync(int amount, Guid accountId, string description)
        {
            Deposit? transaction = await _accountSavingRepository.AddDepositAsync(amount, accountId, description);
            return _mapper.Map<GetDeposit>(transaction);
        }

        public async Task<GetWithdraw?> WithDrawAsync(int amount, Guid accountId)
        {
            Withdraw? transaction = await _accountSavingRepository.WithDrawAsync(amount, accountId);
            return _mapper.Map<GetWithdraw>(transaction);
        }
        public async Task<GetTransfer?> TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transactionType)
        {
            Transfer? transaction = await _accountSavingRepository.TransferFunds(fromAccountId, toAccountId, description, transferAmount, transactionType);
            return _mapper.Map<GetTransfer>(transaction);
        }

        public async Task<DeletedAccountSavingResponse?> CloseAccountSavingAsync(Guid accountId)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.CloseAccountSavingAsync(accountId);
            return _mapper.Map<DeletedAccountSavingResponse>(accountSaving);
        }

        public async Task<CreatedAccountSavingResponse?> CreateAccountSavingAsync(CreateAccountSaving createAccountSaving)
        {
            var account = new AccountSaving
            {
                ClientId = createAccountSaving.ClientId,
                CurrentBalance = createAccountSaving.CurrentBalance,
                Currency = createAccountSaving.Currency,
            };
            AccountSaving? newAccount = await _accountSavingRepository.CreateAsync(account);
            return _mapper.Map<CreatedAccountSavingResponse>(newAccount);
        }

        public async Task<DeletedAccountSavingResponse?> DeleteAccountSavingAsync(Guid accountId)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.DeleteAsync(accountId);
            return _mapper.Map<DeletedAccountSavingResponse>(accountSaving);
        }

        public async Task<GetAccountSaving?> GetAccountSavingByAccountNumberAsync(long accountNumber)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.GetAccountbyAccountNumber(accountNumber);
            return _mapper.Map<GetAccountSaving>(accountSaving);
        }

        public async Task<GetAccountSaving?> GetAccountSavingByIdAsync(Guid accountId)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.GetbyIdAsync(accountId);
            return _mapper.Map<GetAccountSaving>(accountSaving);
        }

        public async Task<List<GetAccountSaving>> GetAllAccountsAsync()
        {
            List<AccountSaving>? accountSaving = await _accountSavingRepository.GetAllAsync();
            return accountSaving.Select(st => _mapper.Map<GetAccountSaving>(st)).ToList();
        }

        public async Task<Paginated<GetAccountSaving>> GetPaginatedAccountsAsync(int page, int pageSize)
        {
            IQueryable<AccountSaving> queryable = _accountSavingRepository.GetAllQueryable();
            Paginated<AccountSaving> paginatedResult = await _accountSavingRepository.GetPaginatedAccountsAsync(queryable, page, pageSize);

            List<GetAccountSaving> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetAccountSaving>(st)).ToList():
                [];

            return new Paginated<GetAccountSaving>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<GetTransaction>> GetPaginatedTransactionsByAccountAsync(Guid accountId, int page, int pageSize)
        {
            IQueryable<Transaction> queryable = _accountSavingRepository.GetTransactionsByAccountQueryable(accountId);

            Paginated<Transaction> paginatedResult = await _accountSavingRepository.GetTransactionsPaginatedAsync(queryable, page, pageSize);

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

        public async Task<List<GetTransaction>?> GetTransactionsHistory(Guid accountId)
        {
            List<Transaction>? transactions = await _accountSavingRepository.GetTransactionsHistory(accountId);
            return transactions?.Select(st => _mapper.Map<GetTransaction>(st)).ToList();
        }

        public async Task<UpdatedAccountSavingResponse?> UpdateAccountSavingAsync(Guid id, UpdateAccountSaving updateAccountSaving)
        {

            var account = new AccountSaving
            {
                AccountName = updateAccountSaving.AccountName,
                AccountStatus = updateAccountSaving.AccountStatus,
            };
            var newAccount = await _accountSavingRepository.UpdateAsync(id, account);
            return _mapper.Map<UpdatedAccountSavingResponse>(newAccount);
        }


    }
}
