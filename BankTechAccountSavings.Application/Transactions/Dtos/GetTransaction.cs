using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class GetTransaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public AccountSaving? DestinationProduct { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ConfirmationNumber { get; set; }
        public long Voucher { get; set; }
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int Commission { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
    }
}
