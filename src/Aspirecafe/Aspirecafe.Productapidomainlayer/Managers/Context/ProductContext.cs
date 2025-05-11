using AspireCafe.ProductApiDomainLayer.Managers.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AspireCafe.ProductApiDomainLayer.Managers.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions options) : base(options) { }

        internal DbSet<ProductDomainModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDomainModel>()
                .HasPartitionKey(x => x.DocumentType)
                .ToContainer("products")
                .HasKey(x => x.Id);
        }
    }
}
