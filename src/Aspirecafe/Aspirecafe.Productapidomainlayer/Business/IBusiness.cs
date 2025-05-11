using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Results;

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
