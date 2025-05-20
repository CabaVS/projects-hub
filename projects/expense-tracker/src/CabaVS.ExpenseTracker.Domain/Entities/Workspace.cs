using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Workspace : Entity
{
    private readonly List<WorkspaceMember> _members;
    
    public WorkspaceName Name { get; private set; }
    public IReadOnlyCollection<WorkspaceMember> Members => _members.AsReadOnly();
    
    private Workspace(Guid id, WorkspaceName name, List<WorkspaceMember> members) : base(id)
    {
        Name = name;
        _members = members;
    }

    public static Result<Workspace> CreateNew(string name, User creator) =>
        CreateExisting(Guid.NewGuid(), name, [])
            .Tap(w => w._members.Add(new WorkspaceMember(w, creator, true)));

    public static Result<Workspace> CreateExisting(Guid id, string name, IEnumerable<WorkspaceMember> members) =>
        WorkspaceName.Create(name)
            .Map(x => new Workspace(id, x, [.. members]));

    public Result Rename(string name, User author) =>
        Result.Success()
            .Ensure(() => _members.Exists(wm => wm.User == author && wm.IsAdmin), WorkspaceErrors.AdminPermissionsRequired())
            .Bind(() => WorkspaceName.Create(name))
            .Tap(x => Name = x);

    public Result AddMember(User user, bool isAdmin) =>
        Result.Success()
            .Ensure(() => !_members.Exists(wm => wm.User == user), WorkspaceErrors.MemberAlreadyExist(user.Id))
            .Tap(() => _members.Add(new WorkspaceMember(this, user, isAdmin)));
}
