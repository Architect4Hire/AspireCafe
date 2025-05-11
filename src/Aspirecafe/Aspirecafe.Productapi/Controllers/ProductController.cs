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



        [HttpGet("{productId:guid}")]
        public async Task<Result<ProductServiceModel>> FetchProductById(Guid productId)
        {
            var result = await _facade.FetchProductByIdAsync(productId);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<ProductServiceModel>.Failure(error, result.Messages)
            );
        }


    }
}
