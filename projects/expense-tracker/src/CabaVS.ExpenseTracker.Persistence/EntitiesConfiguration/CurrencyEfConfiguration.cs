using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.EntitiesConfiguration;

internal sealed class CurrencyEfConfiguration : IEntityTypeConfiguration<CurrencyEfEntity>
{
    public void Configure(EntityTypeBuilder<CurrencyEfEntity> builder)
    {
        builder.ToTable(
            "Currencies", 
            t => t.HasCheckConstraint(
                "CK_Currency_Code_MinLength", 
                $"LEN([Code]) >= {CurrencyCode.MinLength}"));
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(CurrencyName.MaxLength)
            .IsRequired();
        
        builder.Property(x => x.Code)
            .HasMaxLength(CurrencyCode.MaxLength)
            .IsRequired();
        
        builder.Property(x => x.Symbol)
            .HasMaxLength(CurrencySymbol.MaxLength)
            .IsRequired();
    }
}
