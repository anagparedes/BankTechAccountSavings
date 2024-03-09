namespace BankTechAccountSavings.Domain.Entities
{
    public sealed class Deposit: Transaction
    {
        public AccountSaving? DestinationProduct { get; set; }
        public Guid DestinationProductId { get; set; }
        public decimal Credit { get; set; }
    }
}
