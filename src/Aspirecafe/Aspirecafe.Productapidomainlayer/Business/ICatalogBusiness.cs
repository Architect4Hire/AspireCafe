using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public interface ICatalogBusiness
    {
        Task<CatalogServiceModel> FetchCatalog();
        Task<ProductMetaDataServiceModel> FetchProductMetadataAsync(ProductMetaDataViewModel products);
    }
}
