using AspireCafe.CounterApiDomainLayer.Managers.Models.Enums;
using AspireCafe.Shared.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.View
{
    public class OrderViewModel:ViewModelBase
    {
        public Guid? OrderId { get; set; }
        public OrderType OrderType { get; set; }
        public int? TableNumber { get; set; }
        public string? CustomerName { get; set; }
        public List<LineItemViewModel> Items { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Tip { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? Notes { get; set; }

        public OrderViewModel()
        {
            Items = new List<LineItemViewModel>();
        }
    }
}
