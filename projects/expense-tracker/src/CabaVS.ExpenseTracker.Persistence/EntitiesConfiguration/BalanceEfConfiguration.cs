using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.EntitiesConfiguration;

internal sealed class BalanceEfConfiguration : IEntityTypeConfiguration<BalanceEfEntity>
{
    public void Configure(EntityTypeBuilder<BalanceEfEntity> builder)
    {
        builder.ToTable("Balances");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(BalanceName.MaxLength)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();
        
        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(x => x.Workspace)
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
