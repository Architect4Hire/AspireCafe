using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;

namespace AspireCafe.BaristaApiDomainLayer.Data
{
    public interface IData
    {
        Task<ProcessingOrderDomainModel> AddOrderAsync(ProcessingOrderDomainModel order);
        Task<ProcessingOrderDomainModel> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus);
        Task<List<ProcessingOrderDomainModel>> FetchActiveOrdersAsync();
    }
}
