using AspireCafe.ProductApiDomainLayer.Data;
using AspireCafe.ProductApiDomainLayer.Managers.Extensions;
using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public class CatalogBusiness : ICatalogBusiness
    {
        private readonly ICatalogData _data;

        public CatalogBusiness(ICatalogData data)
        {
            _data = data;
        }

        public async Task<CatalogServiceModel> FetchCatalog()
        {
            var data = await _data.FetchCatalog();
            return data.MapToServiceModel();
        }

        public async Task<ProductMetaDataServiceModel> FetchProductMetadataAsync(ProductMetaDataViewModel products)
        {
            var data = await _data.FetchProductMetadataAsync(products);
            return data.MapToMetadataServiceModel();
        }
    }
}
