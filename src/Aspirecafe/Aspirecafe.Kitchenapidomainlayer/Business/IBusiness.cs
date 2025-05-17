using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;
using AspireCafe.Shared.Models.Service.OrderUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.KitchenApiDomainLayer.Business
{
    public interface IBusiness
    {
        Task<OrderUpdateServiceModel> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus);
        Task<OrderGridServiceModel> FetchActiveOrdersAsync();
        Task<OrderUpdateServiceModel> AddOrderAsync(ProcessingOrderDomainModel order);
    }
}
