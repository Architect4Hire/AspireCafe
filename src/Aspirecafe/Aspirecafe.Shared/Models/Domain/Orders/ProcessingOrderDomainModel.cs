using AspireCafe.Shared.Enums;

namespace AspireCafe.Shared.Models.Domain.Orders
{
    public class ProcessingOrderDomainModel : DomainBaseModel
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public OrderProcessStatus ProcessStatus { get; set; }
        public OrderProcessStation CurrentStation { get; set; }
        public List<OrderProcessingStatusHistory> History { get; set; }

        public ProcessingOrderDomainModel()
        {
            History = new List<OrderProcessingStatusHistory>();
        }
    }
}
