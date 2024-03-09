using BankTechAccountSavings.Application.AccountSavings.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.AccountSavings.Validators
{
    public class CreateAccountSavingValidator:AbstractValidator<CreateAccountSaving>
    {
        public CreateAccountSavingValidator()
        {
            RuleFor(account => account.ClientId)
            .NotEmpty().WithMessage("Client ID is required.");

            RuleFor(account => account.CurrentBalance)
                .GreaterThan(0).WithMessage("Current balance must be greater than zero.");

            RuleFor(account => account.Currency)
                .IsInEnum().WithMessage("Invalid currency.");
        }
    }
}
