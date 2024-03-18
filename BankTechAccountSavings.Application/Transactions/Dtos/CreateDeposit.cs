using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateDeposit
    {
        public long DestinationProductNumber { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string? Description { get; set; }
    }
}
