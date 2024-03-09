using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Entities
{
    public sealed class Transfer: Transaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public AccountSaving? DestinationProduct { get; set; }
        public Guid SourceProductId { get; set; }
        public Guid DestinationProductId { get; set; }
        public TransferType TransferType { get; set; }
        public int Commission { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
}
