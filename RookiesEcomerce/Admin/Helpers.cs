using IdentityModel.AspNetCore.OAuth2Introspection;
using System.Security.Claims;

namespace Admin
{
    public static class Helpers
    {
        public static async Task OnTokenValidated(TokenValidatedContext context)
        {
            var identity = context.Principal.Identity as ClaimsIdentity;
            identity.AddClaim(new Claim("role", "role"));
        }
    }
}
