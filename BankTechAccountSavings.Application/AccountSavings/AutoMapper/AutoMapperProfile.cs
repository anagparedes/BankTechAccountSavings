using AutoMapper;
using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Application.AccountSavings.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateAccountSaving, AccountSaving>();
            CreateMap<AccountSaving, CreateAccountSaving>();

            CreateMap<CreateAccountSaving, CreatedAccountSavingResponse>();
            CreateMap<CreatedAccountSavingResponse, CreateAccountSaving>();

            CreateMap<CreatedAccountSavingResponse, CreateAccountSaving>();
            CreateMap<CreateAccountSaving, CreatedAccountSavingResponse>();

            CreateMap<CreatedAccountSavingResponse, AccountSaving>();
            CreateMap<AccountSaving, CreatedAccountSavingResponse>();

            CreateMap<DeletedAccountSavingResponse, AccountSaving>();
            CreateMap<AccountSaving, DeletedAccountSavingResponse>();

            CreateMap<GetAccountSaving, AccountSaving>();
            CreateMap<AccountSaving, GetAccountSaving>();

            CreateMap<GetTransaction, AccountSaving>();
            CreateMap<AccountSaving, GetTransaction>();

            CreateMap<UpdateAccountSaving, AccountSaving>();
            CreateMap<AccountSaving, UpdateAccountSaving>();

            CreateMap<UpdatedAccountSavingResponse, AccountSaving>();
            CreateMap<AccountSaving, UpdatedAccountSavingResponse>();

            CreateMap<GetBeneficiary, Beneficiary>();
            CreateMap<Beneficiary, GetBeneficiary>();

        }
    }
}
