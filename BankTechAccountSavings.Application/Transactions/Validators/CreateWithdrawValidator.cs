using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateWithdrawValidator : AbstractValidator<CreateWithdraw>
    {
        public CreateWithdrawValidator()
        {
            RuleFor(withdraw => withdraw.SourceProductId)
                .NotEmpty().WithMessage("Source product ID is required.");

            RuleFor(withdraw => withdraw.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
