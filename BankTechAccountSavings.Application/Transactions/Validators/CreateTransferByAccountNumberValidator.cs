using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateTransferByAccountNumberValidator : AbstractValidator<CreateTransferByAccountNumber>
    {
        public CreateTransferByAccountNumberValidator()
        {
            RuleFor(x => x.DestinationProductNumber).NotEmpty().WithMessage("Destination Product Number is required.").GreaterThan(0).WithMessage("The destination product number must be greater than zero.");
            RuleFor(x => x.SourceProductNumber).NotEmpty().WithMessage("Source Product Number is required.").GreaterThan(0).WithMessage("The source product number must be greater than zero.");
            RuleFor(x => x.TransferType).IsInEnum().WithMessage("Invalid transfer type.");
            RuleFor(x => x.Description).MaximumLength(255).WithMessage("The description cannot exceed 255 characters.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("The amount must be greater than zero.");
        }
    }
}
