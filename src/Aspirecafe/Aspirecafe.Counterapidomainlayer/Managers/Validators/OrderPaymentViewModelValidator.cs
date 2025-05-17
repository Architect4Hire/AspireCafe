using AspireCafe.Shared.Models.View.Counter;
using FluentValidation;

namespace AspireCafe.CounterApiDomainLayer.Managers.Validators
{
    public class OrderPaymentViewModelValidator : AbstractValidator<OrderPaymentViewModel>
    {
        public OrderPaymentViewModelValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("Order ID cannot be empty.")
                .NotEqual(Guid.Empty)
                .WithMessage("Order ID cannot be empty.");
            RuleFor(x => x.PaymentMethod).IsInEnum();
            RuleFor(x => x.CheckAmount)
                .GreaterThan(0)
                .WithMessage("Check amount must be greater than 0.");
            RuleFor(x => x.TipAmount)
                .GreaterThan(0)
                .WithMessage("Tip amount must be greater than 0.");
            RuleFor(x => x.CheckAmount >= x.SubTotal)
                .Equal(true)
                .WithMessage("Check amount must be greater than or equal to the total amount.");
        }
    }
}
