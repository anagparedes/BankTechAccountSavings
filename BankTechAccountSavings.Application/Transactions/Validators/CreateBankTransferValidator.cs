using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateBankTransferValidator : AbstractValidator<CreateBankTransfer>
    {
        public CreateBankTransferValidator()
        {
            RuleFor(x => x.SourceProductNumber).NotEmpty().WithMessage("El número de cuenta de origen es requerido.");
            RuleFor(x => x.DestinationProductNumber).NotEmpty().WithMessage("El número de cuenta de destino es requerido.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("La descripción es requerida.");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("El monto es requerido.")
                .GreaterThan(0).WithMessage("El monto debe ser mayor que cero.");
        }
    }
}
