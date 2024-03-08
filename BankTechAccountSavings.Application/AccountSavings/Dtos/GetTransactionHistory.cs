using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class GetTransactionHistory
    {
        public long AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public List<Transfer> TransactionHistory { get; } = [];
    }
}
