using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;

namespace CabaVS.ExpenseTracker.Presentation.UserContext;

internal sealed class DummyCurrentUserAccessor : ICurrentUserAccessor
{
    private readonly UserModel _currentUser = new(
        Guid.Parse(
            Environment.GetEnvironmentVariable("CVS_USER_ID")
            ?? throw new InvalidOperationException("Environment variable with Current User ID is not found.")),
        "Test User",
        true);

    public Guid GetCurrentUserId() => _currentUser.Id;

    public UserModel GetCurrentUser() => _currentUser;

    public bool TryGetCurrentUser(out UserModel userModel)
    {
        userModel = _currentUser;
        return true;
    }
}
