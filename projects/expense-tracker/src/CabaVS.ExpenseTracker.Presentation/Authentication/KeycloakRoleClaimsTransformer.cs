using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace CabaVS.ExpenseTracker.Presentation.Authentication;

internal sealed class KeycloakRoleClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity?)principal.Identity;
        if (identity is not { IsAuthenticated: true })
        {
            return Task.FromResult(principal);
        }

        Claim? realmAccessClaim = identity.FindFirst("realm_access");
        if (realmAccessClaim is null)
        {
            return Task.FromResult(principal);
        }

        using var doc = JsonDocument.Parse(realmAccessClaim.Value);
        if (!doc.RootElement.TryGetProperty("roles", out JsonElement rolesElement) ||
            rolesElement.ValueKind != JsonValueKind.Array)
        {
            return Task.FromResult(principal);
        }

        foreach (JsonElement role in rolesElement.EnumerateArray())
        {
            var roleName = role.GetString();
            if (!string.IsNullOrWhiteSpace(roleName) && !identity.HasClaim(ClaimTypes.Role, roleName))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
            }
        }

        return Task.FromResult(principal);
    }
}
