using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class CreateAccountSaving
    {
        public int ClientId { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
    }
}
