using AspireCafe.BaristaApiDomainLayer.Data;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;
using AspireCafe.Shared.Models.Service.OrderUpdate;
using AspireCafe.BaristaApiDomainLayer.Managers.Extensions;

namespace AspireCafe.BaristaApiDomainLayer.Business
{
    public class Business : IBusiness
    {
        private readonly IData _data;

        public Business(IData data)
        {
            _data = data;
        }

        public async Task<OrderUpdateServiceModel> AddOrderAsync(ProcessingOrderDomainModel order)
        {
            var data = await _data.AddOrderAsync(order);
            return data.MapToServiceModel();
        }

        public async Task<OrderGridServiceModel> FetchActiveOrdersAsync()
        {
            var data = await _data.FetchActiveOrdersAsync();
            return data.MapToServiceModel();
        }

        public async Task<OrderUpdateServiceModel> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus)
        {
            var data = await _data.UpdateOrderStatusAsync(orderId, orderProcessStation, orderProcessStatus);
            return data.MapToServiceModel();
        }
    }
}
