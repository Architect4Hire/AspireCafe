using AspireCafe.ProductApiDomainLayer.Facade;
using AspireCafe.ProductApiDomainLayer.Managers.Models.Service;
using AspireCafe.ProductApiDomainLayer.Managers.Models.View;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AspireCafe.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IFacade _facade;

        public ProductController(IFacade facade)
        {
            _facade = facade;
        }

        /// <summary>
        /// Fetches a product by its unique identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="ProductServiceModel"/> if the operation is successful.
        /// Returns a failure result with error details if the operation fails.
        /// </returns>
        /// <response code="200">Returns the product details successfully.</response>
        /// <response code="404">Indicates that the product was not found.</response>
        /// <response code="500">Indicates an internal server error.</response>
        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 404)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]
        public async Task<Result<ProductServiceModel>> FetchProductById(Guid productId)
        {
            var result = await _facade.FetchProductByIdAsync(productId);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
            );
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product details to be created.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="ProductServiceModel"/> if the operation is successful.
        /// Returns a failure result with error details if the operation fails.
        /// </returns>
        /// <response code="201">Indicates that the product was created successfully.</response>
        /// <response code="400">Indicates a bad request due to invalid input.</response>
        /// <response code="500">Indicates an internal server error.</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 201)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 400)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]

        public async Task<Result<ProductServiceModel>> CreateProduct(ProductViewModel product)
        {
            var result = await _facade.CreateProductAsync(product);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
            );
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The updated product details.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="ProductServiceModel"/> if the operation is successful.
        /// Returns a failure result with error details if the operation fails.
        /// </returns>
        /// <response code="200">Indicates that the product was updated successfully.</response>
        /// <response code="400">Indicates a bad request due to invalid input.</response>
        /// <response code="404">Indicates that the product was not found.</response>
        /// <response code="500">Indicates an internal server error.</response>
        [HttpPut("update")]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 400)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 404)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]

        public async Task<Result<ProductServiceModel>> UpdateProduct(ProductViewModel product)
        {
            var result = await _facade.UpdateProductAsync(product);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
            );
        }

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to be deleted.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="ProductServiceModel"/> if the operation is successful.
        /// Returns a failure result with error details if the operation fails.
        /// </returns>
        /// <response code="200">Indicates that the product was deleted successfully.</response>
        /// <response code="404">Indicates that the product was not found.</response>
        /// <response code="500">Indicates an internal server error.</response>
        [HttpDelete("delete/{productId:guid}")]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 404)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]

        public async Task<Result<ProductServiceModel>> DeleteProduct(Guid productId)
        {
            var result = await _facade.DeleteProductAsync(productId);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
            );
        }


    }
}
