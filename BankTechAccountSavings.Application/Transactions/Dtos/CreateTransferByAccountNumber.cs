using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateTransferByAccountNumber
    {
        public long DestinationProductNumber { get; set; }
        public long SourceProductNumber { get; set; }
        public TransferType TransferType { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
