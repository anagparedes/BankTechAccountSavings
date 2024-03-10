namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateWithdraw
    {
        public Guid SourceProductId { get; set; }
        public decimal Amount { get; set; }
    }
}
