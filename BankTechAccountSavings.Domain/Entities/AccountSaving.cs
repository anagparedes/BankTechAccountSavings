using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Models;

namespace BankTechAccountSavings.Domain.Entities
{
    public class AccountSaving: BaseEntity
    {
        public int ClientId { get; set; }
        public int AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal AnualTaxes { get; set; }
        public Currency Currency { get; set; }
        public DateOnly DateOpened { get; set; }
        public DateOnly? DateClosed { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
