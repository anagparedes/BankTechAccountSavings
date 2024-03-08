using BankTechAccountSavings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankTechAccountSavings.Infraestructure.Context
{
    public interface IAccountSavingDbContext : IDbContext { }

    public class AccountSavingDbContext(DbContextOptions<AccountSavingDbContext> options) : BaseDbContext(options), IAccountSavingDbContext
    {
        public DbSet<AccountSaving> AccountSavings { get; set; }
        public DbSet<Transfer> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.SourceProduct)
                .WithMany(acc => acc.TransactionsAsSource)
                .HasForeignKey(t => t.SourceProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.DestinationProduct)
                .WithMany(acc => acc.TransactionsAsDestination)
                .HasForeignKey(t => t.DestinationProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountSaving>()
                .HasMany(acc => acc.TransactionsAsSource)
                .WithOne(t => t.SourceProduct)
                .HasForeignKey(t => t.SourceProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountSaving>()
                .HasMany(acc => acc.TransactionsAsDestination)
                .WithOne(t => t.DestinationProduct)
                .HasForeignKey(t => t.DestinationProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
