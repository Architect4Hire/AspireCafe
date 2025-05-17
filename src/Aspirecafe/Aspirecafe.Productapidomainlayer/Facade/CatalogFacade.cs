using AspireCafe.ProductApiDomainLayer.Business;
using AspireCafe.Shared.Cache;
using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;
using AspireCafe.Shared.Results;
using Microsoft.Extensions.Caching.Distributed;

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
            var metadata = await _business.FetchProductMetadataAsync(products);
            return Result<ProductMetaDataServiceModel>.Success(metadata);
        }
    }
}
