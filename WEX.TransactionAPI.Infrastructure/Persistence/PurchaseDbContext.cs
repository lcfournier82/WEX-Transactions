using Microsoft.EntityFrameworkCore;
using WEX.TransactionAPI.Application.Interfaces;
using WEX.TransactionAPI.Domain.Entities;
using WEX.TransactionAPI.Domain.ValueObjects;

namespace WEX.TransactionAPI.Infrastructure.Persistence
{
    public class PurchaseDbContext(DbContextOptions<PurchaseDbContext> options) : DbContext(options), IApplicationDbContext
    {
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configures the Purchase entity
            modelBuilder.Entity<Purchase>(builder =>
            {
                builder.HasKey(p => p.Id);

                // Configure the PurchaseDescription Value Object
                builder.Property(p => p.Description)
                    .HasConversion(
                        p => p.Value, // To database
                        v => new PurchaseDescription(v)) // From database
                    .HasMaxLength(PurchaseDescription.MaxLength)
                    .IsRequired();

                // Configure the UsdAmount Value Object
                builder.Property(p => p.Amount)
                    .HasConversion(
                        a => a.Value, // To database
                        v => new UsdAmount(v)) // From database
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Here you could add logic for dispatching domain events,
            // or updating 'DateCreated'/'DateModified' properties
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
