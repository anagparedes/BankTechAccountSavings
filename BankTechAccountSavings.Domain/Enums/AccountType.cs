using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum AccountType
    {
        [Display(Name = "Ahorro")]
        Ahorro = 1,
        [Display(Name = "Corriente")]
        Corriente
    }
}
