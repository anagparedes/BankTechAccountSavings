using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Application.AccountSaving.Dtos;
using BankTechAccountSavings.Application.AccountSaving.Interfaces;
using AutoMapper;

namespace BankTechAccountSavings.Application.AccountSaving.Services
{
    public class AccountSavingService( IAccountSavingRepository accountSavingRepository, IMapper mapper ) : IAccountSavingService

    {
        private readonly IAccountSavingRepository _accountSavingRepository = accountSavingRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CreatedAccountSavingResponse?> AddAccountSavingAsync(CreateAccountSaving createAccountSaving)
        {
            var account = new AccountSavings
            {

               
            };
            var newAccount = await _accountSavingRepository.AddAsync(account);
                return _mapper.Map<CreatedAccountSavingResponse>(newAccount);
           
          

        }

        public Task<DeletedAccountSavingResponse?> DeleteAccountSavingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UpdatedAccountSavingResponse?> UpdateAccountSavingAsync(int id, CreateAccountSaving updateAccountSaving)
        {
            throw new NotImplementedException();
        }

        Task<GetAccountSaving?> IAccountSavingService.GetAccountSavingByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<GetAccountSaving>> IAccountSavingService.GetAllAccountSavingsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
