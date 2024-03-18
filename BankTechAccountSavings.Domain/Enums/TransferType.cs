using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum TransferType
    {
        [Display(Name = "Propia")]
        Propia = 1,
        [Display(Name = "Tercero")]
        Tercero
    }
}
