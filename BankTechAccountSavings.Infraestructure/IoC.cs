using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Repositories.AccountSaving;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankTechAccountSavings.Infraestructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountSavingRepository, AccountSavingRepository>();
        }

    }
}
