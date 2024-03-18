using System.ComponentModel.DataAnnotations;


namespace BankTechAccountSavings.Domain.Enums
{
    public enum AccountStatus
    {
        [Display(Name = "Pendiente")]
        Pendiente = 1,
        [Display(Name = "Activa")]
        Activa,
        [Display(Name = "Cerrada")]
        Cerrada
    }
}
