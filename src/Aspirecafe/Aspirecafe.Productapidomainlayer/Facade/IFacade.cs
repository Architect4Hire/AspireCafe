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
    public interface IFacade
    {
        Task<Result<ProductServiceModel>> FetchProductByIdAsync(Guid productId);
        Task<Result<ProductServiceModel>> CreateProductAsync(ProductViewModel product);
        Task<Result<ProductServiceModel>> UpdateProductAsync(ProductViewModel product);
        Task<Result<ProductServiceModel>> DeleteProductAsync(Guid productId);
    }
}
