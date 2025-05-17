using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public interface IBusiness
    {
        Task<ProductServiceModel> FetchProductByIdAsync(Guid productId);
        Task<ProductServiceModel> CreateProductAsync(ProductViewModel product);
        Task<ProductServiceModel> UpdateProductAsync(ProductViewModel product);
        Task<ProductServiceModel> DeleteProductAsync(Guid productId);
    }
}
