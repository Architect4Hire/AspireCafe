using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Data
{
    public class Data : IData
    {

        public async Task<CatalogServiceModel> FetchCatalog()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductServiceModel> FetchProductByIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductMetaDataServiceModel> FetchProductMetadataAsync(ProductMetaDataViewModel products)
        {
            throw new NotImplementedException();
        }
    }
}
