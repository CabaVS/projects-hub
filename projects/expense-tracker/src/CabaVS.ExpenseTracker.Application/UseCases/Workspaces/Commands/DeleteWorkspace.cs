using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Workspaces.Commands;

public sealed record DeleteWorkspaceCommand(Guid WorkspaceId)
    : IWorkspaceAdminAuthorizationRequest, IRequest<Result>;

internal sealed class DeleteWorkspaceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteWorkspaceCommand, Result>
{
    public async Task<Result> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Workspace workspace = await unitOfWork.Workspaces.GetByIdAsync(request.WorkspaceId, cancellationToken)
                              ?? throw new InvalidOperationException("Workspace not found.");

        await unitOfWork.Workspaces.DeleteAsync(workspace, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
