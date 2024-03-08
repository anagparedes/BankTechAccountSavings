using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateTransaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public AccountSaving? DestinationProduct { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
