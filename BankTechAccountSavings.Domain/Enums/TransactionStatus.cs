using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum TransactionStatus
    {
        [Display(Name = "Completed")]
        Completed = 1,
        [Display(Name = "Failed")]
        Failed,

    }
}
