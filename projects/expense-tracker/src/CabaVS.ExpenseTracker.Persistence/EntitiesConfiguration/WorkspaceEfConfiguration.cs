using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.EntitiesConfiguration;

internal sealed class WorkspaceEfConfiguration : IEntityTypeConfiguration<WorkspaceEfEntity>
{
    public void Configure(EntityTypeBuilder<WorkspaceEfEntity> builder)
    {
        builder.ToTable("Workspaces");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(WorkspaceName.MaxLength)
            .IsRequired();
    }
}
