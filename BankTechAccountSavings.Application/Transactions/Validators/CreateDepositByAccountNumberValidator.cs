using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateDepositByAccountNumberValidator : AbstractValidator<CreateDepositByAccountNumber>
    {
        public CreateDepositByAccountNumberValidator()
        {
           
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("The amount must be greater than zero.");
            RuleFor(x => x.DestinationProductNumber).NotEmpty().WithMessage("Account Number is required.").GreaterThan(0).WithMessage("The Account number must be greater than zero.");
            RuleFor(x => x.Description).MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
        }
    }
}
