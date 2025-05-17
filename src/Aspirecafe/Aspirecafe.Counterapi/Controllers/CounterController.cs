using AspireCafe.CounterApiDomainLayer.Facade;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AspireCafe.CounterApi.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly IFacade _facade;

        public CounterController(IFacade facade)
        {
            _facade = facade;
        }

        /// <summary>
        /// Submits a new order to the system for processing.
        /// </summary>
        /// <param name="order">The order details containing order type, customer information, and line items.</param>
        /// <returns>
        /// A Result object containing:
        /// - Success state if the order was submitted successfully
        /// - Failure state with appropriate error code if submission failed
        /// </returns>
        /// <remarks>
        /// This endpoint handles the initial order creation process. The order goes through validation 
        /// before being processed. Possible error cases include:
        /// - InvalidInput: When order details are missing or invalid
        /// - InternalServerError: When there is a system error processing the order
        /// </remarks>
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status500InternalServerError)]
        [HttpPost("SubmitOrder")]
        public async Task<Result<OrderServiceModel>> SubmitOrder(OrderViewModel order)
        {
            var result = await _facade.SubmitOrderAsync(order);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
            );
        }

        /// <summary>
        /// Retrieves the details of an order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to retrieve.</param>
        /// <returns>
        /// A <see cref="Result{OrderServiceModel}"/> object containing:
        /// - The order details if the operation is successful.
        /// - An error code and messages if the operation fails.
        /// </returns>
        /// <remarks>
        /// This endpoint fetches the details of an existing order. Possible error cases include:
        /// - <see cref="Error.NotFound"/>: When the specified order does not exist.
        /// - <see cref="Error.InternalServerError"/>: When there is a system error retrieving the order.
        /// </remarks>
        /// <response code="200">Returns the order details if found.</response>
        /// <response code="500">Returns an error if there is a server issue.</response>
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status500InternalServerError)]
        [HttpGet("GetOrder/{orderId:guid}")]
        public async Task<Result<OrderServiceModel>> GetOrder(Guid orderId)
        {
            var result = await _facade.GetOrderAsync(orderId);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
            );
        }

        /// <summary>
        /// Submits an existing order to the system for processing.
        /// </summary>
        /// <param name="order">The order details containing order type, customer information, and line items.</param>
        /// <returns>
        /// A Result object containing:
        /// - Success state if the order was submitted successfully
        /// - Failure state with appropriate error code if submission failed
        /// </returns>
        /// <remarks>
        /// This endpoint handles the initial order creation process. The order goes through validation 
        /// before being processed. Possible error cases include:
        /// - InvalidInput: When order details are missing or invalid
        /// - InternalServerError: When there is a system error processing the order
        /// </remarks>
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status500InternalServerError)]
        [HttpPut("UpdateOrder")]
        public async Task<Result<OrderServiceModel>> UpdateOrder(OrderViewModel order)
        {
            var result = await _facade.UpdateOrderAsync(order);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
            );
        }

        /// <summary>
        /// Processes the payment for an existing order.
        /// </summary>
        /// <param name="model">The payment details, including the order ID, payment method, and amounts.</param>
        /// <returns>
        /// A <see cref="Result{OrderServiceModel}"/> object containing:
        /// - The updated order details if the payment is processed successfully.
        /// - An error code and messages if the payment processing fails.
        /// </returns>
        /// <remarks>
        /// This endpoint handles the payment process for an order. Possible error cases include:
        /// - <see cref="Error.InvalidInput"/>: When the payment details are missing or invalid.
        /// - <see cref="Error.NotFound"/>: When the specified order does not exist.
        /// - <see cref="Error.InternalServerError"/>: When there is a system error processing the payment.
        /// </remarks>
        /// <response code="200">Returns the updated order details if the payment is successful.</response>
        /// <response code="500">Returns an error if there is a server issue.</response>
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<OrderServiceModel>), StatusCodes.Status500InternalServerError)]

        [HttpPost("PayOrder")]
        public async Task<Result<OrderServiceModel>> PayOrder(OrderPaymentViewModel model)
        {
            var result = await _facade.PayOrderAsync(model);
            return result.Match(
                onSuccess: () => result,
                onFailure: error => Result<OrderServiceModel>.Failure(error, result.Messages)
            );
        }

    }
}
