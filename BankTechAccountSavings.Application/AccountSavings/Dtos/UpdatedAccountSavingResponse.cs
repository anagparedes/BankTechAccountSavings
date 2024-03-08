using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class UpdatedAccountSavingResponse
    {
        public int AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
        public DateOnly DateOpened { get; set; }
        public DateOnly? DateClosed { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
