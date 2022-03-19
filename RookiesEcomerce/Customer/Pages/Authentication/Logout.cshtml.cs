using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Customer.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
