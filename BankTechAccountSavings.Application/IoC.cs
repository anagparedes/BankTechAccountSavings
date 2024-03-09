using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Services;
using BankTechAccountSavings.Application.AccountSavings.Validators;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Application.Transactions.Interfaces;
using BankTechAccountSavings.Application.Transactions.Services;
using BankTechAccountSavings.Application.Transactions.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankTechAccountSavings.Application
{
    public static class IoC
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountSavingService, AccountSavingService>()
                .AddScoped<ITransactionService, TransactionService>();
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateAccountSaving>, CreateAccountSavingValidator>();
            services.AddScoped<IValidator<UpdateAccountSaving>, UpdateAccountSavingValidator>();
            services.AddScoped<IValidator<CreateDeposit>, CreateDepositValidator>();
            services.AddScoped<IValidator<CreateWithdraw>, CreateWithdrawValidator>();
            services.AddScoped<IValidator<CreateTransfer>, CreateTransferValidator>();

            return services;
        }

    }
}
