using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Entities
{
    public class AccountSaving
    {
        public int AccountId { get; set; }
        public int AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
        public DateOnly DateOpened { get; set; }
        public DateOnly? DateClosed { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
