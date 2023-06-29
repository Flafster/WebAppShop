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
    }
}
