using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Models;

namespace BankTechAccountSavings.Domain.Entities
{
    internal sealed class Beneficiary : BaseEntity
    {  
        public long AccountNumberAssociate { get; set; }
        //public AccountSaving? BeneficiaryAccountSaving { get; set; }
        public long BeneficiaryAccountNumber { get; set; }
        public AccountSaving? AccountSavingAssociate { get; set; }
        public Bank Bank { get; set; }
        public Currency Currency { get; set; }
        public int ClientId { get; set; }
        public Guid AccountId { get; set; }
        public string? IdentificationCard { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BeneficiaryLastName { get; set; }
        public string? Email { get; set; }
    }
}
