﻿using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Models;

namespace BankTechAccountSavings.Domain.Entities
{
    public sealed class AccountSaving: BaseEntity
    {
        public int ClientId { get; set; }
        public long AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? Bank { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal MonthlyInterestGenerated { get; set; }
        public decimal AnnualInterestProjected { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public decimal MonthlyInterestRate { get; set; }
        public List<Deposit> Deposits { get; } = [];
        public List<Transfer> TransfersAsSource { get; } = [];
        public List<Transfer> TransfersAsDestination { get; } = [];
        public List<Withdraw> WithDraws { get; } = [];
        public Currency Currency { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime DateClosed { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
