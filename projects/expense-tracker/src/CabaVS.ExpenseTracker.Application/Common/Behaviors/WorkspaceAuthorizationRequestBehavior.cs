using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.ReadRepositories;
using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class WorkspaceAuthorizationRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor currentUserAccessor,
    IWorkspaceReadRepository workspaceReadRepository)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkspaceAuthorizationRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!currentUserAccessor.TryGetCurrentUser(out UserModel currentUser))
        {
            return FailedResultFactory.Create<TResponse>(
                UserErrors.CurrentUserIsNotAvailable());
        }

        var userIsMember =
            await workspaceReadRepository.UserIsMemberOfWorkspaceAsync(
                request.WorkspaceId, 
                currentUser.Id,
                cancellationToken); 
        
        return userIsMember
            ? await next(cancellationToken)
            : FailedResultFactory.Create<TResponse>(
                WorkspaceErrors.NotFound(request.WorkspaceId));
    }
}
