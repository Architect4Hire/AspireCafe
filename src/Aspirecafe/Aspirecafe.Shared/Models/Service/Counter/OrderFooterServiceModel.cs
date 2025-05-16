namespace AspireCafe.Shared.Models.Service.Counter
{
    public class OrderFooterServiceModel
    {
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string  PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string? Notes { get; set; }
    }
}
