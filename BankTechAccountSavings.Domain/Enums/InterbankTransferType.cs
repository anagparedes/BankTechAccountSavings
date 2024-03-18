using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum InterbankTransferType
    {
        [Display(Name = "LBTR")]
        LBTR = 1,
        [Display(Name = "ACH")]
        ACH
    }
}
