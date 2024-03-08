using AutoMapper;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechTransactions.Application.Transactions.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTransaction, Transfer>();
            CreateMap<Transfer, CreateTransaction>();

            CreateMap<GetTransaction, Transfer>();
            CreateMap<Transfer, GetTransaction>();
        }
    }
}
