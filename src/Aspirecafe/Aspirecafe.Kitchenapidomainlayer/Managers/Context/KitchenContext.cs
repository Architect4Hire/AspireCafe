using AspireCafe.Shared.Models.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace AspireCafe.KitchenApiDomainLayer.Managers.Context
{
    public class KitchenContext : DbContext
    {
        public KitchenContext(DbContextOptions options) : base(options) { }

        internal DbSet<ProcessingOrderDomainModel> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessingOrderDomainModel>()
                .HasPartitionKey(x => x.DocumentType)
                .ToContainer("kitchen")
                .HasKey(x => x.Id);
        }
    }
}
