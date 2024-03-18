using BankTechAccountSavings.Application.AccountSavings.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.AccountSavings.Validators
{
    public class CreateAccountSavingValidator:AbstractValidator<CreateAccountSaving>
    {
        public CreateAccountSavingValidator()
        {
            RuleFor(account => account.ClientId)
            .NotEmpty().WithMessage("El Id del cliente es requerido.");

            RuleFor(account => account.Currency)
                .IsInEnum().WithMessage("Moneda inválida.");
        }
    }
}
