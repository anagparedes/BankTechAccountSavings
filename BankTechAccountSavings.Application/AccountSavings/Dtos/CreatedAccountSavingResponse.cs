using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class CreatedAccountSavingResponse
    {
        public int AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
        public DateOnly DateOpened { get; set; }
        public DateOnly? DateClosed { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
