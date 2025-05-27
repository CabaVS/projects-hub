using System.Security.Claims;
using CabaVS.ExpenseTracker.Application.Abstractions.UserContext;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.Presentation.UserContext;

internal sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Guid GetCurrentUserId() =>
        Guid.TryParse(GetClaimValue(ClaimTypes.NameIdentifier), out Guid userId)
            ? userId
            : throw new InvalidOperationException("Invalid User Id.");

    public UserModel GetCurrentUser() =>
        new(
            GetCurrentUserId(),
            GetClaimValue("name") ?? string.Empty,
            httpContextAccessor.HttpContext?.User.IsInRole("app_admin") == true);

    public bool TryGetCurrentUser(out UserModel userModel)
    {
        if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
        {
            userModel = GetCurrentUser();
            return true;
        }
        
        userModel = new UserModel(Guid.Empty, string.Empty, false);
        return false;
    }

    private string? GetClaimValue(string claimType) => httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
}
