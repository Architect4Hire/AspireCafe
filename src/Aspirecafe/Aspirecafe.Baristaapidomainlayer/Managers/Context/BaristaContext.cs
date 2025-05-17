using AspireCafe.Shared.Models.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace AspireCafe.CounterApiDomainLayer.Managers.Context
{
    public class BaristaContext : DbContext
    {
        public BaristaContext(DbContextOptions options) : base(options) { }

        internal DbSet<ProcessingOrderDomainModel> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessingOrderDomainModel>()
                .HasPartitionKey(x => x.DocumentType)
                .ToContainer("barista")
                .HasKey(x => x.Id);
        }
    }
}
