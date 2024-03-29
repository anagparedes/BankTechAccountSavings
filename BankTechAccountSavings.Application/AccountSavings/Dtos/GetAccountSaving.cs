﻿using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class GetAccountSaving
    {
        public Guid Id { get; set; }
        public int ClientId { get; set; }
        public long AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
