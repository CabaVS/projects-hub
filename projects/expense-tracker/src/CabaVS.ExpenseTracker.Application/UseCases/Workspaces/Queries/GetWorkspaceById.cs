using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Workspaces.Queries;

public sealed record GetWorkspaceByIdQuery(Guid WorkspaceId) 
    : IWorkspaceAuthorizationRequest, IRequest<Result<WorkspaceModel>>;

internal sealed class GetWorkspaceByIdQueryHandler(ICurrentUserAccessor currentUserAccessor, IWorkspaceReadRepository workspaceReadRepository) 
    : IRequestHandler<GetWorkspaceByIdQuery, Result<WorkspaceModel>>
{
    public async Task<Result<WorkspaceModel>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        WorkspaceModel? workspaceModel = await workspaceReadRepository.GetWorkspaceByIdAsync(
            request.WorkspaceId, currentUserAccessor.GetCurrentUserId(), cancellationToken);
        return workspaceModel is not null
            ? workspaceModel
            : WorkspaceErrors.NotFound(request.WorkspaceId);
    }
}

