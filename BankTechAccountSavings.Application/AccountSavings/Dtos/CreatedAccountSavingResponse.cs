using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class CreatedAccountSavingResponse
    {
        public int ClientId { get; set; }
        public long AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
        public DateTime DateOpened { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
