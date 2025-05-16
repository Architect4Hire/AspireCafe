using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;
using AspireCafe.Shared.Results;

namespace AspireCafe.ProductApiDomainLayer.Facade
{
    public interface ICatalogFacade
    {
        Task<Result<CatalogServiceModel>> FetchCatalog();
        Task<Result<ProductMetaDataServiceModel>> FetchProductMetadataAsync(ProductMetaDataViewModel products);
    }
}
