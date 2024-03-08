using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankTechAccountSavings.API.Config
{
    public static class DbConfig
    {
        public static IServiceCollection ConfigDbConnection(this IServiceCollection service, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            service.AddDbContext<AccountSavingDbContext>(options => options.UseSqlServer(connectionString));

            return service;
        }

    }
}
