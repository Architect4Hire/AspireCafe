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
            return Result<CatalogServiceModel>.Success(await _business.FetchCatalog());
        }

        public async Task<Result<ProductServiceModel>> FetchProductByIdAsync(Guid productId)
        {
            return Result<ProductServiceModel>.Success(await _business.FetchProductByIdAsync(productId));
        }

        public async Task<Result<ProductMetaDataServiceModel>> FetchProductMetadataAsync(ProductMetaDataViewModel products)
        {
            // Fix for CS0311: Change the return type to a custom Result wrapper that supports List<T>
            var metadata = await _business.FetchProductMetadataAsync(products);
            return Result<ProductMetaDataServiceModel>.Success(metadata);
        }
    }
}
