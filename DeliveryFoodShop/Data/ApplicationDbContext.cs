﻿using DeliveryFoodShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFoodShop.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        } 

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
