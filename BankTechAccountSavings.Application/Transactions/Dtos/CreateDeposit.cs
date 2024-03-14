namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateDeposit
    {
        public decimal Amount { get; set; }
        public long DestinationProductNumber { get; set; }
        public string? Description { get; set; }
    }
}
