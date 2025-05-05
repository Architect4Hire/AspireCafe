using AspireCafe.CounterApiDomainLayer.Managers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Service
{
    public class OrderFooterServiceModel
    {
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string  PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string? Notes { get; set; }
    }
}
