using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateTransferValidator : AbstractValidator<CreateTransfer>
    {
        public CreateTransferValidator()
        {
            RuleFor(transfer => transfer.SourceProductId)
                .NotNull().WithMessage("Source product is required.");

            RuleFor(transfer => transfer.DestinationProductId)
            .NotNull().WithMessage("Destination product is required.")
            .NotEqual(transfer => transfer.SourceProductId)
                .WithMessage("Source product and destination product must be different.");

            RuleFor(transfer => transfer.TransferType)
                .IsInEnum().WithMessage("Invalid transaction type.");

            RuleFor(transfer => transfer.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(deposit => deposit.Description)
               .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
        }
    }
}
