namespace CabaVS.ExpenseTracker.Application.Common.Requests;

public interface IWorkspaceAdminAuthorizationRequest
{
    Guid WorkspaceId { get; }
}
