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

            CreateMap<CreateTransferByAccountNumber, Transfer>();
            CreateMap<Transfer, CreateTransferByAccountNumber>();

            CreateMap<GetTransfer, Transfer>();
            CreateMap<Transfer, GetTransfer>();

            CreateMap<GetTransferByAccountNumber, Transfer>();
            CreateMap<Transfer, GetTransferByAccountNumber>();

            CreateMap<CreateDeposit, Deposit>();
            CreateMap<Deposit, CreateDeposit>();

            CreateMap<CreateDepositByAccountNumber, Deposit>();
            CreateMap<Deposit, CreateDepositByAccountNumber>();

            CreateMap<GetDeposit, Deposit>();
            CreateMap<Deposit, GetDeposit>();

            CreateMap<GetDepositByAccountNumber, Deposit>();
            CreateMap<Deposit, GetDepositByAccountNumber>();

            CreateMap<CreateWithdraw, Withdraw>();
            CreateMap<Withdraw, CreateWithdraw>();

            CreateMap<CreateWithdrawByAccountNumber, Withdraw>();
            CreateMap<Withdraw, CreateWithdrawByAccountNumber>();

            CreateMap<GetWithdraw, Withdraw>();
            CreateMap<Withdraw, GetWithdraw>();

            CreateMap<GetWithdrawByAccountNumber, Withdraw>();
            CreateMap<Withdraw, GetWithdrawByAccountNumber>();

            CreateMap<GetTransaction, Transaction>();
            CreateMap<Transaction, GetTransaction>();
        }
    }
}
