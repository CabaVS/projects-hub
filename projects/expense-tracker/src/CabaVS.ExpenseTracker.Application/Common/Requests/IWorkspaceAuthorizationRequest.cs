namespace CabaVS.ExpenseTracker.Application.Common.Requests;

public interface IWorkspaceAuthorizationRequest
{
    Guid WorkspaceId { get; }
}
