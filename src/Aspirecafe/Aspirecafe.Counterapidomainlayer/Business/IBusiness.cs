using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;

namespace AspireCafe.CounterApiDomainLayer.Business
{
    public interface IBusiness
    {
        Task<OrderServiceModel> SubmitOrderAsync(OrderViewModel order);
        Task<OrderServiceModel> GetOrderAsync(Guid orderId);
        Task<OrderServiceModel> UpdateOrderAsync(OrderViewModel order);
        Task<OrderServiceModel> PayOrderAsync(OrderPaymentViewModel model);
    }
}
