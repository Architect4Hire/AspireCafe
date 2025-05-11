using AspireCafe.ProductApiDomainLayer.Managers.Models.Enums;
using AspireCafe.Shared.Models.View;

namespace AspireCafe.ProductApiDomainLayer.Managers.Models.View
{
    public class ProductViewModel:ViewModelBase
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public ProductType ProductType { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductSubCategory { get; set; } = string.Empty;
        public ProductStatus ProductStatus { get; set; }
        public RouteType RouteType { get; set; }
    }
}
