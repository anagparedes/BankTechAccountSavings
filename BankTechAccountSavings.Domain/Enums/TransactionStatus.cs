using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum TransactionStatus
    {
        [Display(Name = "Completado")]
        Completado = 1,
        [Display(Name = "Fallido")]
        Fallido,

    }
}
