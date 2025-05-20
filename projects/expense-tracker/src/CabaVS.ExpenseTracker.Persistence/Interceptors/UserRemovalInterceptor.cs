using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CabaVS.ExpenseTracker.Persistence.Interceptors;

internal sealed class UserRemovalInterceptor : ISaveChangesInterceptor
{
    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not ApplicationDbContext dbContext)
        {
            throw new InvalidOperationException($"Expected DbContext to be of type {nameof(ApplicationDbContext)}.");
        }

        IEnumerable<Guid> removedUserIds = dbContext.ChangeTracker
            .Entries<UserEfEntity>()
            .Where(x => x.State == EntityState.Deleted)
            .Select(x => x.Entity.Id)
            .Distinct();
        foreach (Guid userId in removedUserIds)
        {
            Guid[] workspaces = await dbContext.Workspaces
                .Where(w => w.Members!.All(m => m.UserId == userId))
                .Select(w => w.Id)
                .Distinct()
                .ToArrayAsync(cancellationToken);
            dbContext.RemoveRange(workspaces);
        }
        
        return result;
    }
}
