using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Message.Shared;

namespace AspireCafe.Shared.Models.Domain.Orders
{
    public class ProcessingOrderDomainModel : DomainBaseModel
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public OrderProcessStatus ProcessStatus { get; set; }
        public OrderProcessStation CurrentStation { get; set; }
        public List<OrderProcessingStatusHistory> History { get; set; }
        public string CustomerName { get; set; }
        public int TableNumber { get; set; }
        public Dictionary<int, OrderProcessingLineItem> Items { get; set; }

        public ProcessingOrderDomainModel()
        {
            History = new List<OrderProcessingStatusHistory>();
            Items = new Dictionary<int, OrderProcessingLineItem>();
        }
    }
}
