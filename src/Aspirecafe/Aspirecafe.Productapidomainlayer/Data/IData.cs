using AspireCafe.ProductApiDomainLayer.Managers.Models.Domain;

namespace AspireCafe.ProductApiDomainLayer.Data
{
    public interface IData
    {
        Task<ProductDomainModel> FetchProductByIdAsync(Guid productId);
        Task<ProductDomainModel> CreateProductAsync(ProductDomainModel product);
        Task<ProductDomainModel> UpdateProductAsync(ProductDomainModel product);
        Task<ProductDomainModel> DeleteProductAsync(Guid productId);
    }
}
