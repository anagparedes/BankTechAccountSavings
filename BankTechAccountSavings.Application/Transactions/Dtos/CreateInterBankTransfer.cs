using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateInterBankTransfer
    {
        public long SourceProductNumber { get; set; }
        public long DestinationProductNumber { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public TransferType TransferType { get; set; }
        public InterbankTransferType InterbankTransferType { get; set; }
    }
}
