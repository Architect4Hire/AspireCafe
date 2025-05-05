using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Data
{
    public interface IData
    {
        Task<OrderDomainModel> SubmitOrderAsync(OrderDomainModel order);
        Task<OrderDomainModel> GetOrderAsync(Guid orderId);
    }
}
