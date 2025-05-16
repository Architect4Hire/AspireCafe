using AspireCafe.Shared.Enums;

namespace AspireCafe.Shared.Models.Service.Product
{
    public class ProductServiceModel: ServiceBaseModel
    {
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductType ProductType { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
