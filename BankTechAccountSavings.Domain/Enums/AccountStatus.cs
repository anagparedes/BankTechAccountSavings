using System.ComponentModel.DataAnnotations;


namespace BankTechAccountSavings.Domain.Enums
{
    public enum AccountStatus
    {
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Closed")]
        Closed
    }
}
