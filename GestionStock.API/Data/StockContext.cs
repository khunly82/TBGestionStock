using GestionStock.API.Data.Configurations;
using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GestionStock.API.Data
{
    public class StockContext(DbContextOptions options): DbContext(options)
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductConfig());
            //modelBuilder.ApplyConfiguration(new CategoryConfig());
            //modelBuilder.ApplyConfiguration(new ProductPriceConfig());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
