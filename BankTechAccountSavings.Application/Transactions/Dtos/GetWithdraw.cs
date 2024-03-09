using BankTechAccountSavings.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.Transactions.Dtos
{
    public class GetWithdraw
    {
        public Guid Id { get; set; }
        public Guid SourceProductId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ConfirmationNumber { get; set; }
        public long Voucher { get; set; }
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public decimal Amount { get; set; }
    }
}
