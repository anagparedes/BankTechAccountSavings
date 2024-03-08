using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Entities
{
    public sealed class WithDraw: Transaction
    {
        public AccountSaving? SourceProduct { get; set; }
        public Guid SourceProductId { get; set; }
    }
}
