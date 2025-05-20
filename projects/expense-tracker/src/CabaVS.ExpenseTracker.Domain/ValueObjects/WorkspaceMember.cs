using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class WorkspaceMember(Workspace workspace, User user, bool isAdmin) : ValueObject
{
    public Workspace Workspace { get; } = workspace;
    public User User { get; } = user;
    public bool IsAdmin { get; } = isAdmin;
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Workspace.Id;
        yield return User.Id;
        yield return IsAdmin;
    }
}
