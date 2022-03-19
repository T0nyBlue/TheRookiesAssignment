using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Customer.Pages.Authentication
{
    [Authorize]
    public class loginModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("/Index");
        }
    }
}
