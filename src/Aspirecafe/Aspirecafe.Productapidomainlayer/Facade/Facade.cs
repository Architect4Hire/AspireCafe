using AspireCafe.ProductApiDomainLayer.Business;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Facade
{
    public class Facade : IFacade
    {
        private readonly IBusiness _business;
        public Facade(IBusiness business)
        {
            _business = business;
        }

        public async Task<Result<CatalogServiceModel>> FetchCatalog()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ProductServiceModel>> FetchProductByIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<ProductMetaDataServiceModel>>> FetchProductMetadataAsync(ProductMetaDataViewModel products)
        {
            throw new NotImplementedException();
        }
    }
}
