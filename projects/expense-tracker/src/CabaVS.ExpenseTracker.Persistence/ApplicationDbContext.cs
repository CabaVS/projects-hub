using CabaVS.ExpenseTracker.Persistence.Entities;
using CabaVS.ExpenseTracker.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEfEntity> Users { get; set; }
    
    public DbSet<WorkspaceEfEntity> Workspaces { get; set; }
    public DbSet<WorkspaceMemberEfEntity> WorkspaceMembers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyMarker.Assembly);
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.AddInterceptors(
            new UserRemovalInterceptor());
}
