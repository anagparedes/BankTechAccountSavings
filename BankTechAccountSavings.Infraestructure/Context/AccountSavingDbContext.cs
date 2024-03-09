using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankTechAccountSavings.Infraestructure.Context
{
    public interface IAccountSavingDbContext : IDbContext { }

    public class AccountSavingDbContext(DbContextOptions<AccountSavingDbContext> options) : BaseDbContext(options), IAccountSavingDbContext
    {
        public DbSet<AccountSaving> AccountSavings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Withdraw> Withdraws { get; set; }
        public DbSet<Transfer> Transfers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<Transaction>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.SourceProduct)
                .WithMany(acc => acc.TransfersAsSource)
                .HasForeignKey(t => t.SourceProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.DestinationProduct)
                .WithMany(acc => acc.TransfersAsDestination)
                .HasForeignKey(t => t.DestinationProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountSaving>()
                .HasMany(acc => acc.TransfersAsDestination)
                .WithOne(t => t.DestinationProduct)
                .HasForeignKey(t => t.DestinationProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Deposit>()
                .HasOne(t => t.DestinationProduct)
                .WithMany(acc => acc.Deposits)
                .HasForeignKey(t => t.DestinationProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountSaving>()
                .HasMany(acc => acc.Deposits)
                .WithOne(d => d.DestinationProduct)
                .HasForeignKey(d => d.DestinationProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Withdraw>()
               .HasOne(t => t.SourceProduct)
               .WithMany(acc => acc.WithDraws)
               .HasForeignKey(t => t.SourceProductId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountSaving>()
               .HasMany(acc => acc.WithDraws)
               .WithOne(d => d.SourceProduct)
               .HasForeignKey(d => d.SourceProductId)
               .OnDelete(DeleteBehavior.Restrict);

            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(type.ClrType) && type.ClrType != typeof(Transaction))
                {
                    modelBuilder.Entity(type.ClrType).HasQueryFilter(null);
                }
            }
        }

    }
}
