using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Service
{
    public class OrderServiceModel:ServiceBaseModel
    {
        public OrderHeaderServiceModel Header { get; set; }
        public List<OrderLineItemServiceModel> Lines { get; set; }
        public OrderFooterServiceModel Footer { get; set; }

        public OrderServiceModel()
        {
            Header = new OrderHeaderServiceModel();
            Lines = new List<OrderLineItemServiceModel>();
            Footer = new OrderFooterServiceModel();
        }
    }
}
