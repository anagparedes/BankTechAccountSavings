using BankTechAccountSavings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Infraestructure.Context
{
    public interface IAccountSavingDbContext : IDbContext { }

    public class AccountSavingDbContext(DbContextOptions<AccountSavingDbContext> options):BaseDbContext(options), IAccountSavingDbContext
    {
        public DbSet<AccountSavings> AccountSavings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetAssembly(typeof(AccountSavingDbContext));

            if (assembly is not null)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}
