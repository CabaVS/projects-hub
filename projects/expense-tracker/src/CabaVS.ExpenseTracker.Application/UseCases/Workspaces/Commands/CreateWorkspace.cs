using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.UseCases.Workspaces.Commands;

public sealed record CreateWorkspaceCommand(string Name)
    : IRequest<Result<Guid>>;

internal sealed class CreateWorkspaceCommandHandler(ICurrentUserAccessor currentUserAccessor, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWorkspaceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        UserModel currentUser = currentUserAccessor.GetCurrentUser();
        
        User user = await unitOfWork.Users.GetByIdAsync(currentUser.Id, cancellationToken)
            ?? throw new InvalidOperationException("User not found.");
        
        Result<Workspace> result = Workspace.CreateNew(request.Name, user);
        return await result.MapAsync<Workspace, Guid>(async workspace =>
        {
            await unitOfWork.Workspaces.AddAsync(workspace, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return workspace.Id;
        });
    }
}
