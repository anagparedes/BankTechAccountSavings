using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateWithdrawValidator : AbstractValidator<CreateWithdraw>
    {
        public CreateWithdrawValidator()
        {
            RuleFor(withdraw => withdraw.SourceProductNumber)
                .NotEmpty().WithMessage("Account Number is required.").GreaterThan(0).WithMessage("The source product number must be greater than zero.");

            RuleFor(withdraw => withdraw.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
