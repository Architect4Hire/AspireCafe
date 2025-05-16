using AspireCafe.ProductApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Models.View.Product;

namespace AspireCafe.ProductApiDomainLayer.Data
{
    public interface ICatalogData
    {
        Task<List<ProductDomainModel>> FetchCatalog();
        Task<List<ProductDomainModel>> FetchProductMetadataAsync(ProductMetaDataViewModel products);
    }
}
