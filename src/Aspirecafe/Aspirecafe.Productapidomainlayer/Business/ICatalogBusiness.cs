using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Business
{
    public interface ICatalogBusiness
    {
        Task<CatalogServiceModel> FetchCatalog();
        Task<ProductMetaDataServiceModel> FetchProductMetadataAsync(ProductMetaDataViewModel products);
    }
}
