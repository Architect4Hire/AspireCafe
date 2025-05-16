using AspireCafe.Shared.Enums;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Domain
{
    public class OrderHeaderDomainModel
    {
        public OrderType OrderType { get; set; }
        public int? TableNumber { get; set; }
        public string? CustomerName { get; set; }
    }
}
