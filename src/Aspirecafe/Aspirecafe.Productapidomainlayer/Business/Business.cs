using AspireCafe.ProductApiDomainLayer.Data;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public class Business : IBusiness
    {
        private readonly IData _data;

        public Business(IData data)
        {
            _data = data;
        }

        public async Task<CatalogServiceModel> FetchCatalog()
        {
            return await _data.FetchCatalog();
        }

        public async Task<ProductServiceModel> FetchProductByIdAsync(Guid productId)
        {
            return await _data.FetchProductByIdAsync(productId);
        }

        public async Task<ProductMetaDataServiceModel> FetchProductMetadataAsync(ProductMetaDataViewModel products)
        {
            return await _data.FetchProductMetadataAsync(products);
        }
    }
}
