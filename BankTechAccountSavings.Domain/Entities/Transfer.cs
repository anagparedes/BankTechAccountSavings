using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Models;

namespace BankTechAccountSavings.Domain.Entities
{
    public sealed class Transfer: Transaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public AccountSaving? DestinationProduct { get; set; }
        public Guid SourceProductId { get; set; }
        public Guid DestinationProductId { get; set; }
    }
}
