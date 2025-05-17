using AspireCafe.Shared.Models.View.Product;
using FluentValidation;

namespace AspireCafe.ProductApiDomainLayer.Managers.Validators
{
    public class ProductViewModelValidator:AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator()
        {
            RuleFor(x => x.ProductType).IsInEnum();
            RuleFor(x => x.ProductCategory).IsInEnum();
            RuleFor(x => x.ProductSubCategory).IsInEnum();
            RuleFor(x => x.ProductStatus).IsInEnum();
            RuleFor(x => x.RouteType).IsInEnum();
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .Length(1, 100)
                .WithMessage("Name must be between 1 and 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Name is required.")
                .Length(1, 500)
                .WithMessage("Name must be between 1 and 500 characters.");
            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required.")
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");
        }
    }
}
