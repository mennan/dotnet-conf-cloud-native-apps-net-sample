using ECommerce.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Contexts
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Apple Macbook Pro 15'",
                    Description =
                        "The MacBook is carved out of solid aluminum, thus giving it a distinctive look and a grayish-white hue.",
                    Price = 12500,
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    Name = "iPhone 12",
                    Description = "Phone",
                    Price = 5430,
                    IsActive = true
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}