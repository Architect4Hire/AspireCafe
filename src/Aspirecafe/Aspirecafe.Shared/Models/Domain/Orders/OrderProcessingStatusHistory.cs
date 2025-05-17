using AspireCafe.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Models.Domain.Orders
{
    public class OrderProcessingStatusHistory
    {
        public OrderProcessStation PreviousStation { get; set; }
        public OrderProcessStation CurrentStation { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
    }
}
