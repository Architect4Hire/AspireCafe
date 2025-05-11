using AspireCafe.ProductApiDomainLayer.Data;
using AspireCafe.ProductApiDomainLayer.Managers.Extensions;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
