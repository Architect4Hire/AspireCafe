using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AspireCafe.CounterApiDomainLayer.Managers.Context
{
    public class CounterContext : DbContext
    {
        public CounterContext(DbContextOptions options) : base(options) { }

        internal DbSet<OrderDomainModel> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDomainModel>()
                .HasPartitionKey(x => x.DocumentType)
                .ToContainer("orders")
                .HasKey(x => x.Id);
        }
    }
}
