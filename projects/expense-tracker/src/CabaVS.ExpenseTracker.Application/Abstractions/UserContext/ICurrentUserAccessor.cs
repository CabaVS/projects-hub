namespace CabaVS.ExpenseTracker.Application.Abstractions.UserContext;

public interface ICurrentUserAccessor
{
    Guid GetCurrentUserId();
    
    UserModel GetCurrentUser();
    
    bool TryGetCurrentUser(out UserModel userModel);
}
