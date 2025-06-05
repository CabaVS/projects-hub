using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Workspaces.Commands;

public sealed record UpdateWorkspaceCommand(Guid WorkspaceId, string Name) 
    : IWorkspaceAdminAuthorizationRequest, IRequest<Result>;

internal sealed class UpdateWorkspaceCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateWorkspaceCommand, Result>
{
    public async Task<Result> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Workspace workspace = await unitOfWork.Workspaces.GetByIdAsync(request.WorkspaceId, cancellationToken) 
                              ?? throw new InvalidOperationException("Workspace not found.");

        Result result = workspace.Rename(request.Name);
        await result
            .Map(() => workspace)
            .TapAsync(async updated =>
            {
                await unitOfWork.Workspaces.UpdateAsync(updated, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            });

        return result;
    }
}
