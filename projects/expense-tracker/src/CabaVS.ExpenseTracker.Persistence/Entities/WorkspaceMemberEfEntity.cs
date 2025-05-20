namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class WorkspaceMemberEfEntity
{
    public Guid UserId { get; set; }
    public UserEfEntity? User { get; set; }

    public Guid WorkspaceId { get; set; }
    public WorkspaceEfEntity? Workspace { get; set; }

    public bool IsAdmin { get; set; }
}
