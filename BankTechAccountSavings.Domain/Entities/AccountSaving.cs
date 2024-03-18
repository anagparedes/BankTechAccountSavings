using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Models;

namespace BankTechAccountSavings.Domain.Entities
{
    internal sealed class AccountSaving : BaseEntity
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
        public List<Beneficiary> Beneficiaries { get; } = [];
        public Currency Currency { get; set; }
        public DateTimeOffset DateOpened { get; set; }
        public DateTimeOffset DateClosed { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string? AccountType { get; set; }
        public bool IsActive { get; set; }
    }
}
