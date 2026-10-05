using System.Security.Claims;

namespace Resto.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsLoggedIn(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.HasClaim(x => x.Type == ClaimTypes.Name);
    }
}