using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Domain.Entities
{
    internal sealed class Transfer: Transaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public AccountSaving? DestinationProduct { get; set; }
        public Guid SourceProductId { get; set; }
        public Guid DestinationProductId { get; set; }
        public TransferType TransferType { get; set; }
        public int Commission { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set;  }
    }
}
