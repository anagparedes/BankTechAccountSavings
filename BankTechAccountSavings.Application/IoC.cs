using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Services;
using BankTechAccountSavings.Application.AccountSavings.Validators;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Application.Transactions.Interfaces;
using BankTechAccountSavings.Application.Transactions.Services;
using BankTechAccountSavings.Application.Transactions.Validators;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using BankTechAccountSavings.Domain.Enums;
using System.Reflection;
using Microsoft.OpenApi.Any;
using System.ComponentModel.DataAnnotations;

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
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.MapType<AccountStatus>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetValues(typeof(AccountStatus)).Cast<AccountStatus>()
                                                                .Select(value =>
                                                                {
                                                                    var fieldInfo = value.GetType().GetField(value.ToString());
                                                                    var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                                                                    return new OpenApiString((attribute?.Name) ?? value.ToString());
                                                                })
                                                                .ToList<IOpenApiAny>()
                });
                c.MapType<Currency>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetValues(typeof(Currency)).Cast<Currency>()
                                                           .Select(value =>
                                                           {
                                                               var fieldInfo = value.GetType().GetField(value.ToString());
                                                               var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                                                               return new OpenApiString((attribute?.Name) ?? value.ToString());
                                                           })
                                                           .ToList<IOpenApiAny>()
                });
                c.MapType<TransactionStatus>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetValues(typeof(TransactionStatus)).Cast<TransactionStatus>()
                                                                    .Select(value =>
                                                                    {
                                                                        var fieldInfo = value.GetType().GetField(value.ToString());
                                                                        var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                                                                        return new OpenApiString((attribute?.Name) ?? value.ToString());
                                                                    })
                                                                    .ToList<IOpenApiAny>()
                });
                c.MapType<TransferType>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetValues(typeof(TransferType)).Cast<TransferType>()
                                                               .Select(value =>
                                                               {
                                                                   var fieldInfo = value.GetType().GetField(value.ToString());
                                                                   var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                                                                   return new OpenApiString((attribute?.Name) ?? value.ToString());
                                                               })
                                                               .ToList<IOpenApiAny>()
                });
                c.MapType<TransactionType>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>()
                                                                  .Select(value =>
                                                                  {
                                                                      var fieldInfo = value.GetType().GetField(value.ToString());
                                                                      var attribute = fieldInfo?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                                                                      return new OpenApiString((attribute?.Name) ?? value.ToString());
                                                                  })
                                                                  .ToList<IOpenApiAny>()
                });
            });
            return services;
        }

    }
}
