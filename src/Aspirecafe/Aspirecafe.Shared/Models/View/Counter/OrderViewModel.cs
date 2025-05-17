using AspireCafe.Shared.Enums;

namespace AspireCafe.Shared.Models.View.Counter
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
