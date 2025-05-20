using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.EntitiesConfiguration;

internal sealed class WorkspaceMemberEfConfiguration : IEntityTypeConfiguration<WorkspaceMemberEfEntity>
{
    public void Configure(EntityTypeBuilder<WorkspaceMemberEfEntity> builder)
    {
        builder.ToTable("WorkspaceMembers");
        builder.HasKey(x => new { x.UserId, x.WorkspaceId });
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction) // Handled by Interceptor
            .IsRequired();

        builder.HasOne(x => x.Workspace)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(x => x.IsAdmin)
            .IsRequired();
    }
}
