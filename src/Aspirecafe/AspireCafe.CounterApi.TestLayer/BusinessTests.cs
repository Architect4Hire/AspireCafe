using AspireCafe.CounterApiDomainLayer.Business;
using AspireCafe.CounterApiDomainLayer.Data;
using AspireCafe.CounterApiDomainLayer.Managers.Models.Domain;
using AspireCafe.CounterApiDomainLayer.Managers.Models.Enums;
using AspireCafe.CounterApiDomainLayer.Managers.Models.View;
using Moq;

namespace AspireCafe.CounterApi.TestLayer
{
    [TestClass]
    public class BusinessTests
    {
        private Mock<IData> _dataMock;
        private Business _business;

        [TestInitialize]
        public void Setup()
        {
            _dataMock = new Mock<IData>();
            _business = new Business(_dataMock.Object);
        }

        [TestMethod]
        [DataRow(true, DisplayName = "GetOrderAsync - Success")]
        [DataRow(false, DisplayName = "GetOrderAsync - Failure")]
        public async Task GetOrderAsync_ShouldHandleResultCorrectly(bool isSuccess)
        {
            // Arrange
            var orderId = Guid.NewGuid();
            if (isSuccess)
            {
                _dataMock.Setup(d => d.GetOrderAsync(orderId)).ReturnsAsync(new OrderDomainModel());
            }
            else
            {
                _dataMock.Setup(d => d.GetOrderAsync(orderId)).ReturnsAsync((OrderDomainModel)null);
            }

            // Act
            var result = await _business.GetOrderAsync(orderId);

            // Assert
            if (isSuccess)
            {
                Assert.IsNotNull(result);
            }
            else
            {
                Assert.IsNull(result);
            }
            _dataMock.Verify(d => d.GetOrderAsync(orderId), Times.Once);
        }

        [TestMethod]
        [DataRow(true, DisplayName = "PayOrderAsync - Payment Successful")]
        [DataRow(false, DisplayName = "PayOrderAsync - Payment Failed")]
        public async Task PayOrderAsync_ShouldHandleResultCorrectly(bool isPaymentSuccessful)
        {
            // Arrange
            var paymentModel = new OrderPaymentViewModel
            {
                OrderId = Guid.NewGuid(),
                PaymentMethod = PaymentMethod.Card,
                CheckAmount = 100,
                TipAmount = 10
            };

            _dataMock.Setup(d => d.PayOrderAsync(paymentModel.OrderId, paymentModel.PaymentMethod, paymentModel.CheckAmount, paymentModel.TipAmount))
                     .ReturnsAsync(isPaymentSuccessful);

            if (isPaymentSuccessful)
            {
                _dataMock.Setup(d => d.GetOrderAsync(paymentModel.OrderId)).ReturnsAsync(new OrderDomainModel());
            }

            // Act
            var result = await _business.PayOrderAsync(paymentModel);

            // Assert
            if (isPaymentSuccessful)
            {
                Assert.IsNotNull(result);
                _dataMock.Verify(d => d.GetOrderAsync(paymentModel.OrderId), Times.Once);
            }
            else
            {
                Assert.IsNull(result);
                _dataMock.Verify(d => d.GetOrderAsync(It.IsAny<Guid>()), Times.Never);
            }

            _dataMock.Verify(d => d.PayOrderAsync(paymentModel.OrderId, paymentModel.PaymentMethod, paymentModel.CheckAmount, paymentModel.TipAmount), Times.Once);
        }

        [TestMethod]
        [DataRow(true, DisplayName = "SubmitOrderAsync - Success")]
        public async Task SubmitOrderAsync_ShouldReturnMappedOrder(bool isSuccess)
        {
            // Arrange
            var orderViewModel = new OrderViewModel();
            var domainModel = new OrderDomainModel();
            _dataMock.Setup(d => d.SubmitOrderAsync(It.IsAny<OrderDomainModel>())).ReturnsAsync(domainModel);

            // Act
            var result = await _business.SubmitOrderAsync(orderViewModel);

            // Assert
            Assert.IsNotNull(result);
            _dataMock.Verify(d => d.SubmitOrderAsync(It.IsAny<OrderDomainModel>()), Times.Once);
        }

        [TestMethod]
        [DataRow(true, DisplayName = "UpdateOrderAsync - Success")]
        public async Task UpdateOrderAsync_ShouldReturnMappedOrder(bool isSuccess)
        {
            // Arrange
            var orderViewModel = new OrderViewModel();
            var domainModel = new OrderDomainModel();
            _dataMock.Setup(d => d.UpdateOrderAsync(It.IsAny<OrderDomainModel>())).ReturnsAsync(domainModel);

            // Act
            var result = await _business.UpdateOrderAsync(orderViewModel);

            // Assert
            Assert.IsNotNull(result);
            _dataMock.Verify(d => d.UpdateOrderAsync(It.IsAny<OrderDomainModel>()), Times.Once);
        }
    }
}
