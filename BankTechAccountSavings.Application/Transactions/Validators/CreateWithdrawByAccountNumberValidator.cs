using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateWithdrawByAccountNumberValidator: AbstractValidator<CreateWithdrawByAccountNumber>
    {
        public CreateWithdrawByAccountNumberValidator()
        {
            RuleFor(x => x.SourceProductNumber).NotEmpty().WithMessage("Account Number is required.").GreaterThan(0).WithMessage("The source product number must be greater than zero.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("The amount must be greater than zero.");
        }
    }
}
