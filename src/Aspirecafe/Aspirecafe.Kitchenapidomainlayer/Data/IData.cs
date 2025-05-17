using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.KitchenApiDomainLayer.Data
{
    public interface IData
    {
        Task<ProcessingOrderDomainModel> AddOrderAsync(ProcessingOrderDomainModel order);
        Task<ProcessingOrderDomainModel> UpdateOrderStatusAsync(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus);
        Task<List<ProcessingOrderDomainModel>> FetchActiveOrdersAsync();
    }
}
