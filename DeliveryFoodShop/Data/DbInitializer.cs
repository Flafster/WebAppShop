﻿using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFoodShop.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitializer(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager) 
        {
            this.dbContext = dbContext;
            this.roleManager = roleManager;
        }

        public void SeedRoles()
        {
            dbContext.Database.Migrate();
            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole { Name = "Admin" }).GetAwaiter().GetResult();
            }
            if (!roleManager.RoleExistsAsync("Manager").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole { Name = "Manager" }).GetAwaiter().GetResult();
            }
            if (!roleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole { Name = "Customer" }).GetAwaiter().GetResult();
            }
        }

        public static async Task SeedRolesAsync(ApplicationDbContext dbContext)
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
        }
    }
}