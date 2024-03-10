namespace BankTechAccountSavings.Domain.Entities
{
    internal sealed class Deposit: Transaction
    {
        public AccountSaving? DestinationProduct { get; set; }
        public Guid DestinationProductId { get; set; }
        public long DestinationProductNumber { get; set; }
        public decimal Credit { get; set;  }
    }
}
