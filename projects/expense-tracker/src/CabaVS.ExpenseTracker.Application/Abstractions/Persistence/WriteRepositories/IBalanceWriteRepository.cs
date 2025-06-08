using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.WriteRepositories;

public interface IBalanceWriteRepository
{
    Task<Balance?> GetByIdAsync(Guid balanceId, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Guid workspaceId, Balance balance, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid workspaceId, Balance balance, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid workspaceId, Balance balance, CancellationToken cancellationToken = default);
}
