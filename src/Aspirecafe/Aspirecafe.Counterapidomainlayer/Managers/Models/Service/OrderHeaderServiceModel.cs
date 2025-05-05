using AspireCafe.CounterApiDomainLayer.Managers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Service
{
    public class OrderHeaderServiceModel
    {
        public string OrderType { get; set; }
        public int? TableNumber { get; set; }
        public string? CustomerName { get; set; }
    }
}
