using CabaVS.ExpenseTracker.Application.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;

public interface IWorkspaceReadRepository
{
    Task<WorkspaceSlimModel[]> GetAllWorkspacesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<WorkspaceModel?> GetWorkspaceByIdAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);

    Task<bool> UserIsAdminOfWorkspaceAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UserIsMemberOfWorkspaceAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
}
