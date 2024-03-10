using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class GetTransfer
    {
        public Guid Id { get; set; }
        public Guid SourceProductId { get; set; }
        public Guid DestinationProductId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ConfirmationNumber { get; set; }
        public long Voucher { get; set; }
        public string? Description { get; set; }
        public TransferType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int Commission { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
