using AspireCafe.CounterApi.Controllers;
using AspireCafe.CounterApiDomainLayer.Facade;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Service.Counter;
using AspireCafe.Shared.Models.View.Counter;
using AspireCafe.Shared.Results;
using Moq;

namespace AspireCafe.CounterApi.TestLayer
{
    [TestClass]
    public class ControllerTests
    {
        private Mock<IFacade> _facadeMock;
        private CounterController _controller;

        [TestInitialize]
        public void Setup()
        {
            _facadeMock = new Mock<IFacade>();
            _controller = new CounterController(_facadeMock.Object);
        }

        [TestMethod]
        [DataRow(true, null, DisplayName = "SubmitOrder - Success")]
        [DataRow(false, "Invalid order", DisplayName = "SubmitOrder - Failure")]
        public async Task SubmitOrder_ShouldHandleResultCorrectly(bool isSuccess, string errorMessage)
        {
            // Arrange
            var order = new OrderViewModel();
            var result = isSuccess
                ? Result<OrderServiceModel>.Success(new OrderServiceModel())
                : Result<OrderServiceModel>.Failure(Error.InvalidInput, new List<string> { errorMessage });

            _facadeMock.Setup(f => f.SubmitOrderAsync(order)).ReturnsAsync(result);

            // Act
            var response = await _controller.SubmitOrder(order);

            // Assert
            Assert.AreEqual(isSuccess, response.IsSuccess);
            if (!isSuccess)
            {
                Assert.AreEqual(errorMessage, response.Messages[0]);
            }
        }

        [TestMethod]
        [DataRow(true, null, DisplayName = "GetOrder - Success")]
        [DataRow(false, "Order not found", DisplayName = "GetOrder - Failure")]
        public async Task GetOrder_ShouldHandleResultCorrectly(bool isSuccess, string errorMessage)
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var result = isSuccess
                ? Result<OrderServiceModel>.Success(new OrderServiceModel())
                : Result<OrderServiceModel>.Failure(Error.NotFound, new List<string> { errorMessage });

            _facadeMock.Setup(f => f.GetOrderAsync(orderId)).ReturnsAsync(result);

            // Act
            var response = await _controller.GetOrder(orderId);

            // Assert
            Assert.AreEqual(isSuccess, response.IsSuccess);
            if (!isSuccess)
            {
                Assert.AreEqual(errorMessage, response.Messages[0]);
            }
        }

        [TestMethod]
        [DataRow(true, null, DisplayName = "UpdateOrder - Success")]
        [DataRow(false, "Invalid order", DisplayName = "UpdateOrder - Failure")]
        public async Task UpdateOrder_ShouldHandleResultCorrectly(bool isSuccess, string errorMessage)
        {
            // Arrange
            var order = new OrderViewModel();
            var result = isSuccess
                ? Result<OrderServiceModel>.Success(new OrderServiceModel())
                : Result<OrderServiceModel>.Failure(Error.InvalidInput, new List<string> { errorMessage });

            _facadeMock.Setup(f => f.UpdateOrderAsync(order)).ReturnsAsync(result);

            // Act
            var response = await _controller.UpdateOrder(order);

            // Assert
            Assert.AreEqual(isSuccess, response.IsSuccess);
            if (!isSuccess)
            {
                Assert.AreEqual(errorMessage, response.Messages[0]);
            }
        }

        [TestMethod]
        [DataRow(true, null, DisplayName = "PayOrder - Success")]
        [DataRow(false, "Payment failed", DisplayName = "PayOrder - Failure")]
        public async Task PayOrder_ShouldHandleResultCorrectly(bool isSuccess, string errorMessage)
        {
            // Arrange
            var paymentModel = new OrderPaymentViewModel();
            var result = isSuccess
                ? Result<OrderServiceModel>.Success(new OrderServiceModel())
                : Result<OrderServiceModel>.Failure(Error.InvalidInput, new List<string> { errorMessage });

            _facadeMock.Setup(f => f.PayOrderAsync(paymentModel)).ReturnsAsync(result);

            // Act
            var response = await _controller.PayOrder(paymentModel);

            // Assert
            Assert.AreEqual(isSuccess, response.IsSuccess);
            if (!isSuccess)
            {
                Assert.AreEqual(errorMessage, response.Messages[0]);
            }
        }
    }
}

