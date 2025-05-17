using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;
using AspireCafe.Shared.Results;

namespace AspireCafe.CounterApiDomainLayer.Facade
{
    public interface IFacade
    {
        Task<Result<OrderServiceModel>> SubmitOrderAsync(OrderViewModel order);
        Task<Result<OrderServiceModel>> GetOrderAsync(Guid orderId);
        Task<Result<OrderServiceModel>> UpdateOrderAsync(OrderViewModel order);
        Task<Result<OrderServiceModel>> PayOrderAsync(OrderPaymentViewModel model);
    }
}
