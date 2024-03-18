using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class GetTransfer
    {
        public Guid Id { get; set; }
        public Guid SourceProductId { get; set; }
        public long SourceProductNumber { get; set; }
        public Guid DestinationProductId { get; set; }
        public long DestinationProductNumber { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public int ConfirmationNumber { get; set; }
        public long Voucher { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int Commission { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
