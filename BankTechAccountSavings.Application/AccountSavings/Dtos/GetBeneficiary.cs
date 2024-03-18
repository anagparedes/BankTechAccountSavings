
using BankTechAccountSavings.Domain.Enums;

namespace BankTechAccountSavings.Application.AccountSavings.Dtos
{
    public class GetBeneficiary
    {
        public string? BeneficiaryName { get; set; }
        public string? BeneficiaryLastName { get; set; }
        public string? IdentificationCard { get; set; }
        public long BeneficiaryAccountNumber { get; set; }
        public Bank Bank { get; set; }
        public Currency Currency { get; set; }
    }
}
