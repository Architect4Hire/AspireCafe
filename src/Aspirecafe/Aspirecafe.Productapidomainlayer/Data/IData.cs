using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Data
{
    public interface IData
    {
        Task<CatalogServiceModel> FetchCatalog();
        Task<ProductServiceModel> FetchProductByIdAsync(Guid productId);
        Task<ProductMetaDataServiceModel> FetchProductMetadataAsync(ProductMetaDataViewModel products);
    }
}
