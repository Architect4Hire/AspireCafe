using Microsoft.AspNetCore.Http;
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

        [HttpGet("catalog")]
        public async Task<Result<OrderServiceModel>> FetchCatalog(OrderViewModel order)
        {
            var result = await _facade.SubmitOrderAsync(order);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
            );
        }

        [HttpGet("{productId:guid}")]
        public async Task<Result<OrderServiceModel>> FetchProductById(Guid productId)
        {
            var result = await _facade.SubmitOrderAsync(order);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
            );
        }

        [HttpPost("metadata")]
        public async Task<Result<OrderServiceModel>> FetchProductMetadata(ProductMetadataViewModel products)
        {
            var result = await _facade.SubmitOrderAsync(order);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
            );
        }
    }
}
