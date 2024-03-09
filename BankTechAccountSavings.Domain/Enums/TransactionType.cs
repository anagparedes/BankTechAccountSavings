using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Depósito")]
        Deposit = 1,
        [Display(Name = "Retiro")]
        WithDraw,
        [Display(Name = "Transferencia")]
        Transfer
    }
}
