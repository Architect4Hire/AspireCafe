using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Context
{
    public class CounterContext : DbContext
    {
        public CounterContext(DbContextOptions options) : base(options) { }

        internal DbSet<OrderDomainModel> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("AspireCafe");
            modelBuilder.Entity<OrderDomainModel>()
                .HasPartitionKey(x => x.DocumentType)
                .ToContainer("orders")
                .HasKey(x => x.Id);
        }
    }
}
