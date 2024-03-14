using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class GetWithdraw
    {
        public int ClientId { get; set; }
        public string? AccountName { get; set; }
        public long? SourceProductNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public long WithdrawPassword { get; set; }
        public long WithdrawCode { get; set; }
    }
}
