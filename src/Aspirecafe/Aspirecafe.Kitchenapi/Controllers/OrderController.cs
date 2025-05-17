using AspireCafe.BaristaApiDomainLayer.Facade;
using AspireCafe.Shared.Models.Service.OrderUpdate;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Enums;

namespace AspireCafe.KitchenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFacade _facade;

        public OrderController(IFacade facade)
        {
            _facade = facade;
        }

        [HttpPut("update/status/{orderId:guid}/{orderProcessStation:OrderProcessStation}/{orderProcessStatus:OrderProcessStatus}")]
        public async Task<Result<OrderUpdateServiceModel>> UpdateOrderStatus(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus)
        {
            var result = await _facade.UpdateOrderStatusAsync(orderId, orderProcessStation, orderProcessStatus);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderUpdateServiceModel>.Failure(error, result.Messages)
            );
        }

        [HttpGet("grid")]
        public async Task<Result<OrderGridServiceModel>> FetchActiveOrders()
        {
            var result = await _facade.FetchActiveOrdersAsync();
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderGridServiceModel>.Failure(error, result.Messages)
            );
        }
    }
}
