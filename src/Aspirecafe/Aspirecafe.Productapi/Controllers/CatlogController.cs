using AspireCafe.ProductApiDomainLayer.Facade;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using AspireCafe.Shared.Extensions;

namespace AspireCafe.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatlogController : ControllerBase
    {
        private readonly ICatalogFacade _facade;

        public CatlogController(ICatalogFacade facade)
        {
            _facade = facade;
        }

        [HttpGet("fetch")]
        public async Task<Result<CatalogServiceModel>> FetchCatalog()
        {
            var result = await _facade.FetchCatalog();
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<CatalogServiceModel>.Failure(error, result.Messages)
            );
        }

        [HttpPost("fetch/metadata")]
        public async Task<Result<ProductMetaDataServiceModel>> FetchProductMetadata(ProductMetaDataViewModel products)
        {
            var result = await _facade.FetchProductMetadataAsync(products);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<ProductMetaDataServiceModel>.Failure(error, result.Messages)
            );
        }
    }
}
