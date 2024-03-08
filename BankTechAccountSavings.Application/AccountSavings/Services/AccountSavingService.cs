using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using AutoMapper;

namespace BankTechAccountSavings.Application.AccountSavings.Services
{
    public class AccountSavingService( IAccountSavingRepository accountSavingRepository, IMapper mapper ) : IAccountSavingService

    {
        private readonly IAccountSavingRepository _accountSavingRepository = accountSavingRepository;
        private readonly IMapper _mapper = mapper;

        public Task<CreatedAccountSavingResponse?> AddDepositAsync(int money, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CreatedAccountSavingResponse?> CloseAccountSavingAsync(int accountNumber)
        {
            throw new NotImplementedException();
        }

        public Task<CreatedAccountSavingResponse?> CreateAccountSavingAsync(CreateAccountSaving createAccountSaving)
        {
            throw new NotImplementedException();
        }

        public Task<DeletedAccountSavingResponse?> DeleteAccountSavingAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetAccountSaving?> GetAccountSavingByAccountNumberAsync(int accountNumber)
        {
            throw new NotImplementedException();
        }

        public Task<GetAccountSaving?> GetAccountSavingByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GetAccountSaving>> GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UpdatedAccountSavingResponse?> UpdateAccountSavingBalanceAsync(int id, CreateAccountSaving updateAccountSaving)
        {
            throw new NotImplementedException();
        }

        public Task<UpdatedAccountSavingResponse?> UpdateAccountSavingStatusAsync(int id, CreateAccountSaving updateAccountSaving)
        {
            throw new NotImplementedException();
        }

        public Task<CreatedAccountSavingResponse?> WithDrawAsync(int accountNumber, int money)
        {
            throw new NotImplementedException();
        }
    }
}
