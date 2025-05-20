using CabaVS.ExpenseTracker.Domain.Common;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class UserErrors
{
    public static Error CurrentUserIsNotAvailable() =>
        new("User.CurrentUserIsNotAvailable", "Unable to retrieve the current user.");
}
