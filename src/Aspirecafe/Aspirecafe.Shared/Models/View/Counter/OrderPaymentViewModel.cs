using AspireCafe.Shared.Enums;

namespace AspireCafe.Shared.Models.View.Counter
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
