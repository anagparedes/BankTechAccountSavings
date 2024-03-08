using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class UpdatedAccountSavingResponse
    {
        public long AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public string? AccountName { get; set; }
        public Currency Currency { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
