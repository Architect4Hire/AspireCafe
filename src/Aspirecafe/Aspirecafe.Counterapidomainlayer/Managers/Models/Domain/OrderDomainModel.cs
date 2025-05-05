using AspireCafe.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Domain
{
    public class OrderDomainModel:DomainBaseModel
    {
        public Guid OrderId { get; set; }
        public OrderHeaderDomainModel Header { get; set; }
        public List<OrderLineItemDomainModel> LineItems { get; set; }
        public OrderFooterDomainModel Footer { get; set; }

        public OrderDomainModel()
        {
            Header = new OrderHeaderDomainModel();
            LineItems = new List<OrderLineItemDomainModel>();
            Footer = new OrderFooterDomainModel();
        }
    }
}
