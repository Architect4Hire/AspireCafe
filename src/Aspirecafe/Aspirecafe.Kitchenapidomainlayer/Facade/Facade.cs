using AspireCafe.KitchenApiDomainLayer.Business;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;
using AspireCafe.Shared.Models.Service.OrderUpdate;
using AspireCafe.Shared.Results;

namespace AspireCafe.BaristaApiDomainLayer.Facade
{
    public class Facade : IFacade
    {
        private readonly IBusiness _business;

        public Facade(IBusiness business)
        {
            _business = business;
        }

        public async Task<Result<OrderUpdateServiceModel>> AddOrderAsync(ProcessingOrderDomainModel order)
        {
            var data = await _business.AddOrderAsync(order);
            return Result<OrderUpdateServiceModel>.Success(data);
        }

        public async Task<Result<OrderGridServiceModel>> FetchActiveOrdersAsync()
        {
            var data = await _business.FetchActiveOrdersAsync();
            return Result<OrderGridServiceModel>.Success(data);
        }

        public async Task<Result<OrderUpdateServiceModel>> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus)
        {
            if (orderId == Guid.Empty)
            {
                return Result<OrderUpdateServiceModel>.Failure(Error.InvalidInput, new List<string>() { "Invalid Metadata" });
            }
            var result = await _business.UpdateOrderStatusAsync(orderId, orderProcessStation, orderProcessStatus);
            if (result == null)
            {
                return Result<OrderUpdateServiceModel>.Failure(Error.NotFound, new List<string>() { "Order not found." });
            }
            return Result<OrderUpdateServiceModel>.Success(result);
        }
    }
}
