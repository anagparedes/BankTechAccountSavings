namespace BankTechAccountSavings.Domain.Entities
{
    public sealed class Withdraw: Transaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public Guid SourceProductId { get; set; }
        public decimal Debit { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
    }
}
