namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateWithdrawByAccountNumber
    {
        public long SourceProductNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
