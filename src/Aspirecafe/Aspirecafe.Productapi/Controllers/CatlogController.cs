using AspireCafe.ProductApiDomainLayer.Facade;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Fetches the catalog containing product details.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="CatalogServiceModel"/> if the operation is successful.
        /// Returns a failure result with error details if the operation fails.
        /// </returns>
        /// <response code="200">Returns the catalog data successfully.</response>
        /// <response code="400">Indicates a bad request due to invalid input.</response>
        /// <response code="500">Indicates an internal server error.</response>
        [ProducesResponseType(typeof(Result<CatalogServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<CatalogServiceModel>), 400)]
        [ProducesResponseType(typeof(Result<CatalogServiceModel>), 500)]
        [HttpGet("fetch")]
        public async Task<Result<CatalogServiceModel>> FetchCatalog()
        {
            var result = await _facade.FetchCatalog();
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<CatalogServiceModel>.Failure(error, result.Messages)
            );
        }

        /// <summary>
        /// Fetches metadata for the specified products.
        /// </summary>
        /// <param name="products">The view model containing the list of product IDs for which metadata is requested.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="ProductMetaDataServiceModel"/> if the operation is successful.
        /// Returns a failure result with error details if the operation fails.
        /// </returns>
        /// <response code="200">Returns the product metadata successfully.</response>
        /// <response code="400">Indicates a bad request due to invalid input.</response>
        /// <response code="500">Indicates an internal server error.</response>
        [HttpPost("fetch/metadata")]
        [ProducesResponseType(typeof(Result<ProductMetaDataServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<ProductMetaDataServiceModel>), 400)]
        [ProducesResponseType(typeof(Result<ProductMetaDataServiceModel>), 500)]
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
