using AspireCafe.ProductApiDomainLayer.Business;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Cache;
using AspireCafe.Shared.Results;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.ProductApiDomainLayer.Facade
{
    public class CatalogFacade : ICatalogFacade
    {
        private readonly ICatalogBusiness _business;
        private readonly CacheAside _cacheAside;

        public CatalogFacade(ICatalogBusiness business, IDistributedCache cache)
        {
            _business = business;
            _cacheAside = new CacheAside(cache);
        }

        public async Task<Result<CatalogServiceModel>> FetchCatalog()
        {
            // fetch from the redis cache or populate the cache and return the data
            var catalog = await _cacheAside.FetchFromCache<CatalogServiceModel>("aspire-cafe-full-product-catalog", async () =>
            {
                return await _business.FetchCatalog();
            }, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            return Result<CatalogServiceModel>.Success(catalog);
        }

        public async Task<Result<ProductMetaDataServiceModel>> FetchProductMetadataAsync(ProductMetaDataViewModel products)
        {
            // Fix for CS0311: Change the return type to a custom Result wrapper that supports List<T>
            var metadata = await _business.FetchProductMetadataAsync(products);
            return Result<ProductMetaDataServiceModel>.Success(metadata);
        }
    }
}
