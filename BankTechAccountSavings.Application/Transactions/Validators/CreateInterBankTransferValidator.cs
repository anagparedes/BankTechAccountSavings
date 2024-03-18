using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateInterBankTransferValidator : AbstractValidator<CreateInterBankTransfer>
    {
        public CreateInterBankTransferValidator()
        {
            RuleFor(transfer => transfer.SourceProductNumber)
                .NotEmpty().WithMessage("Source Product Number is required.").GreaterThan(0).WithMessage("The source product number must be greater than zero.");

            RuleFor(transfer => transfer.DestinationProductNumber)
            .NotEmpty().WithMessage("Destination Product Number is required.").GreaterThan(0).WithMessage("The destination product number must be greater than zero.")
            .NotEqual(transfer => transfer.SourceProductNumber)
                .WithMessage("Source product and destination product must be different.");

            RuleFor(transfer => transfer.TransferType)
                .IsInEnum().WithMessage("Invalid transfer type.");

            RuleFor(transfer => transfer.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(deposit => deposit.Description)
               .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
        }
    }
}
