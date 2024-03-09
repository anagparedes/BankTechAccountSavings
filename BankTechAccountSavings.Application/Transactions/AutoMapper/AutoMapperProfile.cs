using AutoMapper;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechTransactions.Application.Transactions.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTransfer, Transfer>();
            CreateMap<Transfer, CreateTransfer>();

            CreateMap<GetTransfer, Transfer>();
            CreateMap<Transfer, GetTransfer>();

            CreateMap<CreateDeposit, Deposit>();
            CreateMap<Deposit, CreateDeposit>();

            CreateMap<GetDeposit, Deposit>();
            CreateMap<Deposit, GetDeposit>();

            CreateMap<CreateWithdraw, Withdraw>();
            CreateMap<Withdraw, CreateWithdraw>();

            CreateMap<GetWithdraw, Withdraw>();
            CreateMap<Withdraw, GetWithdraw>();

            CreateMap<GetTransaction, Transaction>();
            CreateMap<Transaction, GetTransaction>();
        }
    }
}
