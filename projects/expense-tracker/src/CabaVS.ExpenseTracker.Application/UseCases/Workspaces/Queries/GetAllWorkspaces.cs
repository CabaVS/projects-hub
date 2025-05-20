using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Common;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Workspaces.Queries;

public sealed record GetAllWorkspacesQuery : IRequest<Result<WorkspaceSlimModel[]>>;

internal sealed class GetAllWorkspacesQueryHandler(ICurrentUserAccessor currentUserAccessor, IWorkspaceReadRepository workspaceReadRepository) 
    : IRequestHandler<GetAllWorkspacesQuery, Result<WorkspaceSlimModel[]>>
{
    public async Task<Result<WorkspaceSlimModel[]>> Handle(GetAllWorkspacesQuery request, CancellationToken cancellationToken)
    {
        WorkspaceSlimModel[] workspaces = await workspaceReadRepository.GetAllWorkspacesAsync(
            currentUserAccessor.GetCurrentUserId(), cancellationToken);
        return workspaces;
    }
}
