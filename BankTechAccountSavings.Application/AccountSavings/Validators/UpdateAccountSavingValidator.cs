using BankTechAccountSavings.Application.AccountSavings.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.AccountSavings.Validators
{
    public class UpdateAccountSavingValidator: AbstractValidator<UpdateAccountSaving>
    {
        public UpdateAccountSavingValidator()
        {
            RuleFor(account => account.AccountName)
            .MaximumLength(255).WithMessage("Account name cannot exceed 255 characters.");

            RuleFor(account => account.AccountStatus)
                .IsInEnum().WithMessage("Invalid account status.");
        }
    }
}
