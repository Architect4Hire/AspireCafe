using AspireCafe.CounterApiDomainLayer.Managers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.View
{
    public class OrderPaymentViewModel
    {
        public Guid OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal SubTotal { get; set; }
        public decimal CheckAmount { get; set; }
        public decimal TipAmount { get; set; }
    }
}
