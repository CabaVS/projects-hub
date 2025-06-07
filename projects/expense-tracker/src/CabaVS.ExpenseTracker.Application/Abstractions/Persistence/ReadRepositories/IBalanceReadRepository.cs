using CabaVS.ExpenseTracker.Application.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;

public interface IBalanceReadRepository
{
    Task<BalanceModel[]> GetAllBalancesAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<BalanceModel?> GetBalanceByIdAsync(Guid balanceId, Guid workspaceId, CancellationToken cancellationToken = default);
}
