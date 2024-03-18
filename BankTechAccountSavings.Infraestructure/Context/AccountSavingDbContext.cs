using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BankTechAccountSavings.Infraestructure.Context
{
    public interface IAccountSavingDbContext : IDbContext { }

    internal class AccountSavingDbContext(DbContextOptions<AccountSavingDbContext> options) : BaseDbContext(options), IAccountSavingDbContext
    {
        internal DbSet<AccountSaving> AccountSavings { get; set; }
        internal DbSet<Transaction> Transactions { get; set; }
        internal DbSet<Deposit> Deposits { get; set; }
        internal DbSet<Withdraw> Withdraws { get; set; }
        internal DbSet<Transfer> Transfers { get; set; }
        internal DbSet<Beneficiary> Beneficiaries {  get; set; } 


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

            modelBuilder.Entity<Beneficiary>()
                .HasOne(b => b.AccountSavingAssociate)
                .WithMany(acc => acc.Beneficiaries)
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountSaving>()
               .HasMany(acc => acc.Beneficiaries)
               .WithOne(b => b.AccountSavingAssociate)
               .HasForeignKey(b => b.AccountId)
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
