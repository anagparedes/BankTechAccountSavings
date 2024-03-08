using AutoMapper;
using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Application.AccountSavings.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateAccountSaving, AccountSaving>();
            CreateMap<AccountSaving, CreateAccountSaving>();

            CreateMap<CreatedAccountSavingResponse, AccountSaving>();
            CreateMap<AccountSaving, CreatedAccountSavingResponse>();

            CreateMap<DeletedAccountSavingResponse, AccountSaving>();
            CreateMap<AccountSaving, DeletedAccountSavingResponse>();

            CreateMap<GetAccountSaving, AccountSaving>();
            CreateMap<AccountSaving, GetAccountSaving>();

            CreateMap<GetTransactionHistory, AccountSaving>();
            CreateMap<AccountSaving, GetTransactionHistory>();

            CreateMap<UpdateAccountSaving, AccountSaving>();
            CreateMap<AccountSaving, UpdateAccountSaving>();

            CreateMap<UpdatedAccountSavingResponse, AccountSaving>();
            CreateMap<AccountSaving, UpdatedAccountSavingResponse>();
        }
    }
}
