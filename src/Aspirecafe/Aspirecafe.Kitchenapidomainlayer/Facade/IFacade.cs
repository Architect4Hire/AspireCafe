using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;
using AspireCafe.Shared.Models.Service.OrderUpdate;
using AspireCafe.Shared.Results;

namespace AspireCafe.BaristaApiDomainLayer.Facade
{
    public interface IFacade
    {
        Task<Result<OrderUpdateServiceModel>> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus);
        Task<Result<OrderGridServiceModel>> FetchActiveOrdersAsync();
        Task<Result<OrderUpdateServiceModel>> AddOrderAsync(ProcessingOrderDomainModel order);
    }
}
