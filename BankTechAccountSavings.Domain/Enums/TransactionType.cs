using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum TransactionType
    {
        [Display(Name = "LBTR")]
        LBTR = 1,
        [Display(Name = "ACH")]
        ACH
    }
}
