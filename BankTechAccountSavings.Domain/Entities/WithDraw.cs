namespace BankTechAccountSavings.Domain.Entities
{
    internal sealed class Withdraw: Transaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public Guid SourceProductId { get; set; }
        public int ClientId { get; set; }
        public string? AccountName { get; set; }
        public long SourceProductNumber { get; set; }
        public long WithdrawPassword { get; set; }
        public long WithdrawCode { get; set; }
        public decimal Debit { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set;  }
    }
}
