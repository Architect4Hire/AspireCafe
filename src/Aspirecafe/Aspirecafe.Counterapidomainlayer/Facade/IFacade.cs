using AspireCafe.CounterApiDomainLayer.Managers.Models.Service;
using AspireCafe.CounterApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Facade
{
    public interface IFacade
    {
        Task<Result<OrderServiceModel>> SubmitOrderAsync(OrderViewModel order);
        Task<Result<OrderServiceModel>> GetOrderAsync(Guid orderId);
    }
}
