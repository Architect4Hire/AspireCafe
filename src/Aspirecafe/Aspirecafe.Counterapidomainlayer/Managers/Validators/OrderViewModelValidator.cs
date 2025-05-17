using AspireCafe.Shared.Models.View.Counter;
using FluentValidation;

namespace AspireCafe.CounterApiDomainLayer.Managers.Validators
{
    public class OrderViewModelValidator : AbstractValidator<OrderViewModel>
    {
        public OrderViewModelValidator()
        {
            RuleFor(x => x.OrderType).IsInEnum();
        }
    }
}
