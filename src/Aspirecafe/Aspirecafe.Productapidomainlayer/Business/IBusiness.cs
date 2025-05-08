using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Results;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public interface IBusiness
    {
        Task<CatalogServiceModel> FetchCatalog();
        Task<ProductServiceModel> FetchProductByIdAsync(Guid productId);
        Task<ProductMetaDataServiceModel> FetchProductMetadataAsync(ProductMetaDataViewModel products);
    }
}
