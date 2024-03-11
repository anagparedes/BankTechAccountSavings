using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using AutoMapper;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Application.Transactions.Dtos;

namespace BankTechAccountSavings.Application.AccountSavings.Services
{
    internal class AccountSavingService(IAccountSavingRepository accountSavingRepository, IMapper mapper) : IAccountSavingService

    {
        private readonly IAccountSavingRepository _accountSavingRepository = accountSavingRepository;
        private readonly IMapper _mapper = mapper;

        async Task<List<GetAccountSaving>> IAccountSavingService.GetAllAccountsAsync()
        {
            List<AccountSaving>? accountSaving = await _accountSavingRepository.GetAllAsync();
            return accountSaving.Select(st => _mapper.Map<GetAccountSaving>(st)).ToList();
        }

        async Task<Paginated<GetAccountSaving>> IAccountSavingService.GetPaginatedAccountsAsync(int page, int pageSize)
        {
            IQueryable<AccountSaving> queryable = _accountSavingRepository.GetAllQueryable();
            Paginated<AccountSaving> paginatedResult = await _accountSavingRepository.GetPaginatedAccountsAsync(queryable, page, pageSize);

            List<GetAccountSaving> result = paginatedResult.Items != null
                ? paginatedResult.Items.Select(st => _mapper.Map<GetAccountSaving>(st)).ToList() :
                [];

            return new Paginated<GetAccountSaving>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        async Task<Paginated<GetTransaction>> IAccountSavingService.GetPaginatedTransactionsByAccountAsync(Guid accountId, int page, int pageSize)
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

        async Task<Paginated<GetTransaction>> IAccountSavingService.GetPaginatedTransactionsByAccountNumberAsync(long accountNumber, int page, int pageSize)
        {
            IQueryable<Transaction> queryable = _accountSavingRepository.GetTransactionsByAccountNumberQueryable(accountNumber);

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

        async Task<CreatedAccountSavingResponse?> IAccountSavingService.CreateAccountSavingAsync(CreateAccountSaving createAccountSaving)
        {
            var account = new AccountSaving
            {
                ClientId = createAccountSaving.ClientId,
                CurrentBalance = createAccountSaving.CurrentBalance,
                AccountType = createAccountSaving.AccountType,
                Currency = createAccountSaving.Currency,
            };
            AccountSaving? newAccount = await _accountSavingRepository.CreateAsync(account);
            return _mapper.Map<CreatedAccountSavingResponse>(newAccount);
        }

        async Task<GetDeposit?> IAccountSavingService.AddDepositAsync(int amount, Guid accountId, string description)
        {
            Deposit? transaction = await _accountSavingRepository.AddDepositAsync(amount, accountId, description);
            return _mapper.Map<GetDeposit>(transaction);
        }

        async Task<GetWithdraw?> IAccountSavingService.WithDrawAsync(int amount, Guid accountId)
        {
            Withdraw? transaction = await _accountSavingRepository.WithDrawAsync(amount, accountId);
            return _mapper.Map<GetWithdraw>(transaction);
        }

        async Task<GetTransfer?> IAccountSavingService.TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transferType)
        {
            Transfer? transaction = await _accountSavingRepository.TransferFunds(fromAccountId, toAccountId, description, transferAmount, transferType);
            return _mapper.Map<GetTransfer>(transaction);
        }

        async Task<GetDepositByAccountNumber?> IAccountSavingService.AddDepositByAccountNumberAsync(int amount, long accountNumber, string description)
        {
            Deposit? transaction = await _accountSavingRepository.AddDepositByAccountNumberAsync(amount, accountNumber, description);
            return _mapper.Map<GetDepositByAccountNumber>(transaction);
        }

        async Task<GetWithdrawByAccountNumber?> IAccountSavingService.WithDrawByAccountNumberAsync(int amount, long accountNumber)
        {
            Withdraw? transaction = await _accountSavingRepository.WithDrawByAccountNumberAsync(amount, accountNumber);
            return _mapper.Map<GetWithdrawByAccountNumber>(transaction);
        }

        async Task<GetTransferByAccountNumber?> IAccountSavingService.TransferFundsByAccountNumberAsync(long fromAccountNumber, long toAccountNumber, string description, int transferAmount, TransferType transferType)
        {
            Transfer? transaction = await _accountSavingRepository.TransferFundsByAccountNumberAsync(fromAccountNumber, toAccountNumber, description, transferAmount, transferType);
            return _mapper.Map<GetTransferByAccountNumber>(transaction);
        }

        async Task<List<GetTransaction>?> IAccountSavingService.GetTransactionsHistory(Guid accountId)
        {
            List<Transaction>? transactions = await _accountSavingRepository.GetTransactionsHistory(accountId);
            return transactions?.Select(st => _mapper.Map<GetTransaction>(st)).ToList();
        }

        async Task<List<GetTransaction>?> IAccountSavingService.GetTransactionsHistoryByAccountNumber(long accountNumber)
        {
            List<Transaction>? transactions = await _accountSavingRepository.GetTransactionsHistoryByAccountNumber(accountNumber);
            return transactions?.Select(st => _mapper.Map<GetTransaction>(st)).ToList();
        }

        async Task<GetAccountSaving?> IAccountSavingService.GetAccountSavingByIdAsync(Guid accountId)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.GetbyIdAsync(accountId);
            return _mapper.Map<GetAccountSaving>(accountSaving);
        }

        async Task<GetAccountSaving?> IAccountSavingService.GetAccountSavingByAccountNumberAsync(long accountNumber)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.GetAccountbyAccountNumber(accountNumber);
            return _mapper.Map<GetAccountSaving>(accountSaving);
        }

        async Task<UpdatedAccountSavingResponse?> IAccountSavingService.UpdateAccountSavingAsync(Guid accountId, UpdateAccountSaving updateAccountSaving)
        {
            var account = new AccountSaving
            {
                AccountName = updateAccountSaving.AccountName,
                AccountStatus = updateAccountSaving.AccountStatus,
            };
            var newAccount = await _accountSavingRepository.UpdateAsync(accountId, account);
            return _mapper.Map<UpdatedAccountSavingResponse>(newAccount);
        }

        async Task<DeletedAccountSavingResponse?> IAccountSavingService.DeleteAccountSavingAsync(Guid accountId, string reasonToCloseAccount)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.DeleteAsync(accountId, reasonToCloseAccount);
            return _mapper.Map<DeletedAccountSavingResponse>(accountSaving);
        }

        public string FormatErrorResponse(string errorMessage)
        {
            return $"\"errorMessage\": \"{errorMessage}\"";
        }
    }
}
