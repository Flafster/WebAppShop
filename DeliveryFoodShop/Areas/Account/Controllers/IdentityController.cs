using FoodDeliveryShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFoodShop.Areas.Account.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register registerModel)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    Email = registerModel.Email,
                    UserName = registerModel.Email,
                };
                var result=await userManager.CreateAsync(user, registerModel.Password);
				await userManager.AddToRoleAsync(user, "Customer");
				if (result.Succeeded)
                {
					
					await signInManager.SignInAsync(user, false);
                    return Redirect("~/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerModel);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
			return Redirect("~/");
		}
        [HttpPost]
        public async Task<IActionResult> Login(Login loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
                if(user.Succeeded)
                {
                    return Redirect("~/");
				}
				ModelState.AddModelError("", "Username or password incorrect");
			}
            return View(loginModel);
        }
    }
}
