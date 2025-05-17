using AspireCafe.Shared.Enums;

namespace AspireCafe.Shared.Models.Service.OrderUpdate
{
    public class OrderUpdateServiceModel:ServiceBaseModel
    {
        public OrderStatus OrderStatus { get; set; }
        public Guid OrderId { get; set; }
        public OrderProcessStation Station { get; set; }
        public OrderProcessStatus CookingStatus { get; set; }
    }
}
