﻿using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BankTechAccountSavings.Application
{
    public static class IoC
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountSavingService, AccountSavingService>();
        }

    }
}
