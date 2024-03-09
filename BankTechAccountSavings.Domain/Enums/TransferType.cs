using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum TransferType
    {
        [Display(Name = "LBTR")]
        LBTR = 1,
        [Display(Name = "ACH")]
        ACH
    }
}
