using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class GetTransaction
    {
        public Guid Id { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public int ConfirmationNumber { get; set; }
        public long Voucher { get; set; }
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public decimal Amount { get; set; }
    }
}
