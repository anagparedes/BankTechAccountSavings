using System.ComponentModel.DataAnnotations;

namespace BankTechAccountSavings.Domain.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Depósito")]
        Deposit = 1,
        [Display(Name = "Retiro")]
        WithDraw,
        [Display(Name = "Transferencia")]
        Transfer
    }
}
