using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class DeletedAccountSavingResponse
    {
        public int AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
