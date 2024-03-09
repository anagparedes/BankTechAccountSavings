using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateWithdraw
    {
        public Guid SourceProductId { get; set; }
        public decimal Amount { get; set; }
    }
}
