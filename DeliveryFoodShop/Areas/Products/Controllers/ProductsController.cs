using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using DeliveryFoodShop.Areas.Account.Controllers;
using DeliveryFoodShop.Data;
using DeliveryFoodShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppShop.Models;

namespace WebAppShop.Areas.Products.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly SignInManager<IdentityUser> signInManager;

        public ProductsController(ApplicationDbContext dbContext, SignInManager<IdentityUser> signInManager) 
        {
            this.dbContext = dbContext;
            this.signInManager = signInManager;
        }

        public List<Product> products = new List<Product>();
        

        public IActionResult Catalog()
        {

            products=dbContext.Product.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddInShopingCard(int id)
        {
            if (signInManager.IsSignedIn(User))
            {
                var customerId = HttpContext.User.Identity.Name;
                var cartItem=dbContext.ShopingCartItems.SingleOrDefault(q=>q.ProductId == id && q.CustomerId==customerId);
                if (cartItem == null)
                {
                    cartItem = new CartItem()
                    {
                        CustomerId = customerId,
                        ProductId = id,
                        Quantity = 1,
                        DateCreated = DateTime.Now,
                    };
                    dbContext.ShopingCartItems.Add(cartItem);
                }
                else { cartItem.Quantity++; }
                dbContext.SaveChanges();
                return View();
            }
            return RedirectToAction("Login", "Identity");
        }
    }
}
