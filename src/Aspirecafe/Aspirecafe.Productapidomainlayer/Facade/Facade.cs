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
    public class Facade : IFacade
    {
        private readonly IBusiness _business;

        public Facade(IBusiness business, IDistributedCache cache)
        {
            _business = business;
        }

        public async Task<Result<ProductServiceModel>> FetchProductByIdAsync(Guid productId)
        {
            return Result<ProductServiceModel>.Success(await _business.FetchProductByIdAsync(productId));
        }
    }
}
