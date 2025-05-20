namespace CabaVS.ExpenseTracker.Application.Abstractions.UserContext;

public sealed record UserModel(Guid Id, string UserName, bool IsAdmin);
