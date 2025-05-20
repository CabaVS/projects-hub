using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class WorkspaceErrors
{
    public static NotFoundError NotFound(Guid id) => 
        new(nameof(Workspace), $"Workspace not found by Id '{id}'.");
    
    public static Error AdminPermissionsRequired() => 
        new($"{nameof(Workspace)}.{nameof(AdminPermissionsRequired)}", "Admin permissions required for such action.");

    public static Error NameIsNullOrWhitespace() =>
        StringError.IsNullOrWhitespace(nameof(Workspace), nameof(Workspace.Name));
    public static Error NameIsTooLong(string? value) => 
        StringError.IsTooLong(nameof(Workspace), nameof(Workspace.Name), WorkspaceName.MaxLength, value);
    
    public static Error MemberAlreadyExist(Guid userId) =>
        new($"{nameof(Workspace)}.{nameof(MemberAlreadyExist)}", $"Member {userId} already exist within a workspace.");
}
