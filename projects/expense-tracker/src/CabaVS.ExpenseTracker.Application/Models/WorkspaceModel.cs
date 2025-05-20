namespace CabaVS.ExpenseTracker.Application.Models;

public sealed record WorkspaceModel(Guid Id, string Name, bool CurrentUserIsAdmin);
