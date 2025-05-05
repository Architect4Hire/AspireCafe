using AspireCafe.CounterApiDomainLayer.Managers.Models.Service;
using AspireCafe.CounterApiDomainLayer.Managers.Models.View;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Business
{
    public interface IBusiness
    {
        Task<OrderServiceModel> SubmitOrderAsync(OrderViewModel order);
        Task<OrderServiceModel> GetOrderAsync(Guid orderId);
    }
}
