using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Models.View.Counter
{
    public class LineItemViewModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
