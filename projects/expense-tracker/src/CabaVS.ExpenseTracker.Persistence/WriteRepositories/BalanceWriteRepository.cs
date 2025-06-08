using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.WriteRepositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.WriteRepositories;

internal sealed class BalanceWriteRepository(ApplicationDbContext dbContext) : IBalanceWriteRepository
{
    public async Task<Balance?> GetByIdAsync(Guid balanceId, CancellationToken cancellationToken = default)
    {
        BalanceEfEntity? balanceEf = await dbContext.Balances
            .AsNoTracking()
            .Include(b => b.Currency)
            .Where(w => w.Id == balanceId)
            .FirstOrDefaultAsync(cancellationToken);
        return balanceEf?.ConvertToDomain();
    }

    public Task<Guid> AddAsync(Guid workspaceId, Balance balance, CancellationToken cancellationToken = default)
    {
        var entity = BalanceEfEntity.ConvertFromDomain(workspaceId, balance);
        
        EntityEntry<BalanceEfEntity> added = dbContext.Balances.Add(entity);
        
        return Task.FromResult(added.Entity.Id);
    }

    public Task UpdateAsync(Guid workspaceId, Balance balance, CancellationToken cancellationToken = default)
    {
        var entity = BalanceEfEntity.ConvertFromDomain(workspaceId, balance);
        
        _ = dbContext.Balances.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid workspaceId, Balance balance, CancellationToken cancellationToken = default)
    {
        var entity = BalanceEfEntity.ConvertFromDomain(workspaceId, balance);
        
        _ = dbContext.Balances.Remove(entity);
        
        return Task.CompletedTask;
    }
}
