using AspireCafe.CounterApiDomainLayer.Business;
using AspireCafe.CounterApiDomainLayer.Managers.Validators;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;
using AspireCafe.Shared.Results;

namespace AspireCafe.CounterApiDomainLayer.Facade
{
    public class Facade : IFacade
    {
        private readonly IBusiness _business;
        private OrderViewModelValidator _validator;
        private OrderPaymentViewModelValidator _paymentValidator;

        public Facade(IBusiness business)
        {
            _business = business;
            _validator = new OrderViewModelValidator();
            _paymentValidator = new OrderPaymentViewModelValidator();
        }

        public async Task<Result<OrderServiceModel>> GetOrderAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                return Result<OrderServiceModel>.Failure(Error.InvalidInput, new List<string>() { "Order ID cannot be empty." });
            }
            var result = await _business.GetOrderAsync(orderId);
            if (result == null)
            {
                return Result<OrderServiceModel>.Failure(Error.NotFound, new List<string>() { "Order not found." });
            }
            return Result<OrderServiceModel>.Success(result);
        }

        public async Task<Result<OrderServiceModel>> PayOrderAsync(OrderPaymentViewModel model)
        {
            var validationResult = await _paymentValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Result<OrderServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var result = await _business.PayOrderAsync(model);
            if (result == null)
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
                return Result<OrderServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var result = await _business.SubmitOrderAsync(order);
            return Result<OrderServiceModel>.Success(result);
        }

        public async Task<Result<OrderServiceModel>> UpdateOrderAsync(OrderViewModel order)
        {
            var validationResult = await _validator.ValidateAsync(order);
            if (!validationResult.IsValid)
            {
                return Result<OrderServiceModel>.Failure(Error.InvalidInput, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var result = await _business.UpdateOrderAsync(order);
            return Result<OrderServiceModel>.Success(result);
        }
    }
}
