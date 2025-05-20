using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class WorkspaceEfEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<WorkspaceMemberEfEntity>? Members { get; set; }

    public static WorkspaceEfEntity ConvertFromDomain(Workspace workspace) => 
        new()
        {
            Id = workspace.Id,
            Name = workspace.Name.Value,
            Members = [.. workspace.Members.Select(
                wm => new WorkspaceMemberEfEntity
                {
                    WorkspaceId = workspace.Id, 
                    UserId = wm.User.Id, 
                    IsAdmin = wm.IsAdmin
                })]
        };

    public Workspace ConvertToDomain()
    {
        Workspace workspace = Workspace.CreateExisting(Id, Name, []).Value;

        foreach (WorkspaceMemberEfEntity member in Members!)
        {
            User user = member.User!.ConvertToDomain();
            workspace.AddMember(user, member.IsAdmin);
        }
        
        return workspace;
    }
}
