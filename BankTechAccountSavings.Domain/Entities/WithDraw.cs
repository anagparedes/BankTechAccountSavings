namespace BankTechAccountSavings.Domain.Entities
{
    internal sealed class Withdraw: Transaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public Guid SourceProductId { get; set; }
        public decimal Debit { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set;  }
    }
}
