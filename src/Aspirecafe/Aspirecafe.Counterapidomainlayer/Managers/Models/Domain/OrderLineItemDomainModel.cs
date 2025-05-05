using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Domain
{
    public class OrderLineItemDomainModel
    {
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }
}
