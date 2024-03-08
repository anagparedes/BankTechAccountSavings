using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class UpdateAccountSaving
    {
        public string? AccountName { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
