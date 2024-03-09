﻿using BankTechAccountSavings.Application.Transactions.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.Transactions.Validators
{
    public class CreateDepositValidator : AbstractValidator<CreateDeposit>
    {
        public CreateDepositValidator()
        {
            RuleFor(deposit => deposit.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(deposit => deposit.DestinationProductId)
                .NotEmpty().WithMessage("Destination product ID is required.");

            RuleFor(deposit => deposit.Description)
                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
        }
    }
}
