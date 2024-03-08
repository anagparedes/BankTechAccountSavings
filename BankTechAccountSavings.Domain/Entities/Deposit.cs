using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Entities
{
    public sealed class Deposit: Transaction
    {
        public AccountSaving? DestinationProduct { get; set; }
        public Guid DestinationProductId { get; set; }
    }
}
