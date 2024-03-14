namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateWithdraw
    {
        public long SourceProductNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
