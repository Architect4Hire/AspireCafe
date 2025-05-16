using AspireCafe.ProductApiDomainLayer.Managers.Context;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Domain;
using AspireCafe.Shared.Models.View.Product;
using Microsoft.EntityFrameworkCore;

namespace AspireCafe.ProductApiDomainLayer.Data
{
    public class CatalogData : ICatalogData
    {
        private readonly ProductContext _context;
        public CatalogData(ProductContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDomainModel>> FetchCatalog()
        {
            return await _context.Products.Where(w=>w.IsAvailable && w.IsActive).ToListAsync();
        }

        public async Task<List<ProductDomainModel>> FetchProductMetadataAsync(ProductMetaDataViewModel products)
        {
            return await _context.Products.Where(w => products.ProductIds.Contains(w.Id) && w.IsAvailable && w.IsActive).ToListAsync();
        }
    }
}
