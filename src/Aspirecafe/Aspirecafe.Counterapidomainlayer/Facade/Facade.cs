using AspireCafe.CounterApiDomainLayer.Business;
using AspireCafe.CounterApiDomainLayer.Managers.Models.Service;
using AspireCafe.CounterApiDomainLayer.Managers.Models.View;
using AspireCafe.CounterApiDomainLayer.Managers.Validators;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Facade
{
    public class Facade : IFacade
    {
        private readonly IBusiness _business;
        private OrderViewModelValidator _validator;

        public Facade(IBusiness business)
        {
            _business = business;
            _validator = new OrderViewModelValidator();
        }

        public async Task<Result<OrderServiceModel>> GetOrderAsync(Guid orderId)
        {
            if(orderId == Guid.Empty)
            {
                return Result<OrderServiceModel>.Failure(Error.InvalidInput, new List<string>() { "Order ID cannot be empty." });
            }
            var result = await _business.GetOrderAsync(orderId);
            if(result == null)
            {
                return Result<OrderServiceModel>.Failure(Error.NotFound, new List<string>() { "Order not found." });
            }
            return Result<OrderServiceModel>.Success(result);
        }

        public async Task<Result<OrderServiceModel>> SubmitOrderAsync(OrderViewModel order)
        {
            var validationResult = await _validator.ValidateAsync(order);
            if (!validationResult.IsValid)
            {
                return Result<OrderServiceModel>.Failure(Error.InvalidInput,validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var result = await _business.SubmitOrderAsync(order);
            return Result<OrderServiceModel>.Success(result);
        }
    }
}
