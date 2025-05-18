using AspireCafe.ProductApiDomainLayer.Facade;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Models.Service.Product;
using AspireCafe.Shared.Models.View.Product;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspireCafe.ProductApi.Controllers
{
    [Authorize]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IFacade _facade;

        public ProductController(IFacade facade)
        {
            _facade = facade;
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing a <see cref="Result{ProductServiceModel}"/>.
        /// </returns>
        /// <response code="200">Product found and returned successfully.</response>
        /// <response code="404">Product not found.</response>
        /// <response code="500">Internal server error occurred while fetching the product.</response>
        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 404)]
        [ProducesResponseType(typeof(Result<ProductServiceModel>), 500)]
        public async Task<IActionResult> FetchProductById(Guid productId)
        {
            var result = await _facade.FetchProductByIdAsync(productId);
            return result.Match();
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

        public async Task<IActionResult> CreateProduct(ProductViewModel product)
        {
            var result = await _facade.CreateProductAsync(product);
            return result.Match();
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

        public async Task<IActionResult> UpdateProduct(ProductViewModel product)
        {
            var result = await _facade.UpdateProductAsync(product);
            return result.Match();
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

        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var result = await _facade.DeleteProductAsync(productId);
            return result.Match();
        }


    }
}
