using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using AutoMapper;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Application.Transactions.Dtos;

namespace BankTechAccountSavings.Application.AccountSavings.Services
{
    public class AccountSavingService( IAccountSavingRepository accountSavingRepository, IMapper mapper ) : IAccountSavingService

    {
        private readonly IAccountSavingRepository _accountSavingRepository = accountSavingRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<GetTransaction?> AddDepositAsync(int amount, Guid accountId, string description, TransactionType transactionType)
        {
            Transfer? transaction = await _accountSavingRepository.AddDepositAsync(amount,accountId, description, transactionType);
            return _mapper.Map<GetTransaction>(transaction);
        }

        public async Task<GetTransaction?> WithDrawAsync(int amount, Guid accountId)
        {
            Transfer? transaction = await _accountSavingRepository.WithDrawAsync(amount, accountId);
            return _mapper.Map<GetTransaction>(transaction);
        }
        public async Task<GetTransaction?> TransferFunds(Guid fromAccountId, Guid toAccountId, int transferAmount, TransactionType transactionType)
        {
            Transfer? transaction = await _accountSavingRepository.TransferFunds(fromAccountId, toAccountId, transferAmount, transactionType);
            return _mapper.Map<GetTransaction>(transaction);
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

        public async Task<List<GetTransaction>?> GetTransactionsHistory(Guid accountId)
        {
            List<Transfer>? transactions = await _accountSavingRepository.GetTransactionsHistory(accountId);
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
