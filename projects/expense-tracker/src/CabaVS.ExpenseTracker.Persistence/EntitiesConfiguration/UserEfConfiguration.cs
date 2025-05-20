using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.EntitiesConfiguration;

internal sealed class UserEfConfiguration : IEntityTypeConfiguration<UserEfEntity>
{
    public void Configure(EntityTypeBuilder<UserEfEntity> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
    }
}
