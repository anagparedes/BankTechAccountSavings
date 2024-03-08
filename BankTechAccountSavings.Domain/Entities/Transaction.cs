using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Entities
{
    public class Transaction: BaseEntity
    {
        public AccountSaving? SourceProduct { get; set; }
        public AccountSaving? DestinationProduct { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ConfirmationNumber { get; set; } /*Esto tiene longitud de 7*/
        public long Voucher {  get; set; }  /*Esto tiene longitud de 10*/
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int Commission { get; set; }
        public double Tax { get; set; }
        public double Total {  get; set; }
    }
}
