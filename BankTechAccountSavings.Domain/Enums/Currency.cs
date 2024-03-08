using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum Currency
    {
        [Display(Name = "DOP")]
        DOP = 1,
        [Display(Name = "USD")]
        USD
    }
}
