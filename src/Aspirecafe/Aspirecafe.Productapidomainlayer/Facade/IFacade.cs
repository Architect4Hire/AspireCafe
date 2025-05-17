using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;
using AspireCafe.Shared.Results;

namespace AspireCafe.ProductApiDomainLayer.Facade
{
    public interface IFacade
    {
        Task<Result<ProductServiceModel>> FetchProductByIdAsync(Guid productId);
        Task<Result<ProductServiceModel>> CreateProductAsync(ProductViewModel product);
        Task<Result<ProductServiceModel>> UpdateProductAsync(ProductViewModel product);
        Task<Result<ProductServiceModel>> DeleteProductAsync(Guid productId);
    }
}
