//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace Customer.Pages.Authentication
//{
//    public class RegisterModel : PageModel
//    {
//        private readonly IMetaIdentityUserService _metaIdentityUserService;

//        [BindProperty]
//        public UserRegistrationDto UserRegistration { get; set; }

//        public RegisterModel(IMetaIdentityUserService metaIdentityUserService)
//        {

//            UserRegistration = new UserRegistrationDto();
//            _metaIdentityUserService = metaIdentityUserService;
//        }
//        public IActionResult OnGet()
//        {
//            if (!User.Identity.IsAuthenticated)
//                return RedirectToPage("/Home/Login");
//            return Page();
//        }


//        public async Task<IActionResult> OnPostRegister()
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await _metaIdentityUserService.Register(UserRegistration, "Customer");


//                if (result.Succeeded)
//                {
//                    TempData["AlertMessage"] = "Register Successfully";
//                    return RedirectToPage("/Auth/Login");
//                }
//                else
//                {
//                    if (result.Errors != null)
//                    {
//                        foreach (var error in result.Errors)
//                        {
//                            ModelState.AddModelError("message", error.Description);
//                        }
//                    }

//                }

//            }
//            return Page();
//        }
//    }
//}
