using BankTechAccountSavings.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class CreateBankTransfer
    {
        public long SourceProductNumber { get; set; }
        public long DestinationProductNumber { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public TransferType TransferType { get; set; }
    }
}
