namespace AspireCafe.Shared.Models.Service.Counter
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
