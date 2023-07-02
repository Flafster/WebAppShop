using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using DeliveryFoodShop.Areas.Account.Controllers;
using DeliveryFoodShop.Data;
using DeliveryFoodShop.Models;
using DeliveryFoodShop.Pages.Account.Views.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppShop.Models;

namespace WebAppShop.Areas.Products.Controllers
{
    [Area("Products")]
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
        public List<CartItem> cartItems = new List<CartItem>();
        public List<CartItemForView> cartItemForViews = new List<CartItemForView>();

        public IActionResult Catalog()
        {

            products=dbContext.Product.ToList();
            return View(products);
        }

        public IActionResult ShopingCart() 
        {
            string customerId = HttpContext.User.Identity.Name.ToString();
            cartItems =dbContext.ShopingCartItems.Where(q=>q.CustomerId==customerId).ToList();
            foreach (var item in cartItems)
            {
                products.Add(dbContext.Product.First(q=>q.Id==item.ProductId));
            }
            foreach(var item in products)
            {
                CartItem cartItem = (CartItem)dbContext.ShopingCartItems.First(q => q.CustomerId == customerId && q.ProductId == item.Id);
                cartItemForViews.Add(new CartItemForView
                {
                    Id=cartItem.Id,
                    CustomerId = customerId,
                    ProductId = item.Id,
                    ProductDescription = item.Description,
                    ProductImage = item.Image,
                    ProductName = item.Name,
                    ProductPrice = item.Price*cartItem.Quantity,
                    ProductCategory = item.Category,
                    ProductQuantity = cartItem.Quantity,
                    DataCreatedCartItem = cartItem.DateCreated,
                });
            }
            return View(cartItemForViews);
        }

        public IActionResult Delete(int id)
        {
            var cart=dbContext.ShopingCartItems.First(q => q.Id == id);
            dbContext.ShopingCartItems.Remove(cart);
            dbContext.SaveChanges();
            return RedirectToAction("ShopingCart", "Products");
        }

        
        public IActionResult AddInShopingCard(int id)
        {
            if (signInManager.IsSignedIn(User))
            {
                var customerId = HttpContext.User.Identity.Name;
                var cartItem=dbContext.ShopingCartItems.SingleOrDefault(q=>q.CustomerId==customerId && q.ProductId == id);
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
                return RedirectToAction("Catalog");
            }
            return Redirect("/Account/Identity/Login");
        }
    }
}
