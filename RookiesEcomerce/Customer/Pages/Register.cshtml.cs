using AutoMapper;
using DataAccess.DTO.MyUserDto;
using DataAccess.Model;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Customer.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        

        [BindProperty]
        public CreateUserDto UserRegistration { get; set; }
        public UserManager<MyUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public IMapper Mapper { get; }

        public RegisterModel(UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {

            UserRegistration = new CreateUserDto();
           
            UserManager = userManager;
            RoleManager = roleManager;
            Mapper = mapper;
        }
        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostRegister()
        {
            if (ModelState.IsValid)
            {
                var role = "customer";
                var roleCheck = await RoleManager.FindByNameAsync("customer");
                
              
                    var user = Mapper.Map<MyUser>(UserRegistration);
                    user.UserName = UserRegistration.Email;
                    user.PhoneNumberConfirmed = user.EmailConfirmed = true;
                    user.UserName = user.NormalizedUserName = user.Email;

                var result = await UserManager.CreateAsync(user, UserRegistration.Password);

                    result = UserManager.AddToRoleAsync(user, role).Result;

                    result =
                    UserManager.AddClaimsAsync(
                        user,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, user.UserName),
                            new Claim(JwtClaimTypes.Role, role)
                        }
                    ).Result;
                if (result.Succeeded)
                {
                    TempData["AlertMessage"] = "Register Successfully";
                    return RedirectToPage("/authentication/login");
                }
                else
                {
                    if (result.Errors != null)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("message", error.Description);
                        }
                    }

                }

            }
            return Page();
        }
        
    }
}
