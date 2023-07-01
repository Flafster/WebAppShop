using System.Data;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeliveryFoodShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DeliveryFoodShop.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public DbInitializer(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) 
        {
            this.dbContext = dbContext;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        public static async Task Seed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();

            var roles = new RoleStore<IdentityRole>(dbContext);
            if(!dbContext.Roles.Any(q=>q.Name=="Admin"))
            {
                await roles.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if(!dbContext.Roles.Any(q=>q.Name=="Manager"))
            {
                await roles.CreateAsync(new IdentityRole { Name = "Manager" });
            }
            if(!dbContext.Roles.Any(q=>q.Name=="Customer"))
            {
                await roles.CreateAsync(new IdentityRole { Name = "Customer" });
            }

            var admin = new IdentityUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                NormalizedUserName="ADMIN@ADMIN.COM",
                NormalizedEmail="ADMIN@ADMIN.COM"
            };
            if (!dbContext.Users.Any(q => q.Email == admin.Email))
            {
                var password = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin1!");
                admin.PasswordHash = password;
                dbContext.Users.Add(admin);

                var AdminRole = new IdentityUserRole<string>()
                {
                    RoleId = dbContext.Roles.First(c => c.Name == "Admin").Id,
                    UserId = admin.Id
                };
                dbContext.UserRoles.Add(AdminRole);
                dbContext.SaveChanges();
            }

            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var downloadString = webClient.DownloadString("https://fakestoreapi.com/products");
            string replace = downloadString.Replace("title", "name");
            var products = JsonConvert.DeserializeObject<List<Product>>(replace);

            foreach(var item in products)
            {
                if (!dbContext.Category.Any(q => q.Name == item.Category))
                {
                    dbContext.Category.Add(new Category { Name = item.Category });
                    dbContext.SaveChanges();
                }
            }   


            foreach(var item in products)
            {
                if(!dbContext.Product.Any(q => q.Name == item.Name))
                {
                    var product=new Product 
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        Image = item.Image,
                        Category = item.Category,
                    };
                    dbContext.Product.Add(product);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
