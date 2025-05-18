using AspireCafe.KitchenApiDomainLayer.Facade;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Models.Service.OrderUpdate;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspireCafe.KitchenApi.Controllers
{
    [Authorize]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFacade _facade;

        public OrderController(IFacade facade)
        {
            _facade = facade;
        }

        /// <summary>
        /// Updates the status of an order at a specific process station.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to update.</param>
        /// <param name="orderProcessStation">The station where the order is being processed.</param>
        /// <param name="orderProcessStatus">The new status to set for the order at the station.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing a <see cref="Result{OrderUpdateServiceModel}"/>.
        /// </returns>
        /// <response code="200">Order status updated successfully.</response>
        /// <response code="400">Invalid input provided for updating the order status.</response>
        /// <response code="401">Unauthorized to update the order status.</response>
        /// <response code="403">Forbidden from updating the order status.</response>
        /// <response code="404">Order not found.</response>
        /// <response code="500">Internal server error occurred while updating the order status.</response>
        [HttpPut("update/status/{orderId:guid}/{orderProcessStation:OrderProcessStation}/{orderProcessStatus:OrderProcessStatus}")]
        [ProducesResponseType(typeof(Result<OrderUpdateServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<OrderUpdateServiceModel>), 400)]
        [ProducesResponseType(typeof(Result<OrderUpdateServiceModel>), 401)]
        [ProducesResponseType(typeof(Result<OrderUpdateServiceModel>), 403)]
        [ProducesResponseType(typeof(Result<OrderUpdateServiceModel>), 404)]
        [ProducesResponseType(typeof(Result<OrderUpdateServiceModel>), 500)]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, OrderProcessStation orderProcessStation, OrderProcessStatus orderProcessStatus)
        {
            var result = await _facade.UpdateOrderStatusAsync(orderId, orderProcessStation, orderProcessStatus);
            return result.Match();
        }

        /// <summary>
        /// Retrieves a grid of all active orders currently being processed in the kitchen.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing a <see cref="Result{OrderGridServiceModel}"/>.
        /// </returns>
        /// <response code="200">Active orders retrieved successfully.</response>
        /// <response code="401">Unauthorized to fetch active orders.</response>
        /// <response code="403">Forbidden from fetching active orders.</response>
        /// <response code="500">Internal server error occurred while fetching active orders.</response>
        [HttpGet("grid")]
        [ProducesResponseType(typeof(Result<OrderGridServiceModel>), 200)]
        [ProducesResponseType(typeof(Result<OrderGridServiceModel>), 401)]
        [ProducesResponseType(typeof(Result<OrderGridServiceModel>), 403)]
        [ProducesResponseType(typeof(Result<OrderGridServiceModel>), 500)]
        public async Task<IActionResult> FetchActiveOrders()
        {
            var result = await _facade.FetchActiveOrdersAsync();
            return result.Match();
        }
    }
}