using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Data
{
    public class ApplicationDbContext: IdentityDbContext    // изменили наследование
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<MyModel> MyModel { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}

