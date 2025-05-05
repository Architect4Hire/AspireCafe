using AspireCafe.CounterApiDomainLayer.Managers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Domain
{
    public class OrderFooterDomainModel
    {
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? Notes { get; set; }
        public decimal SettledAmount { get; set; }
        public decimal TipAmount { get; set; }  
    }
}
