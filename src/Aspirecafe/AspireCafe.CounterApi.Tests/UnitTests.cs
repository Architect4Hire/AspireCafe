using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspireCafe.CounterApi.Controllers;

namespace AspireCafe.CounterApi.Tests
{
    [TestClass]
    public class CounterControllerTests
    {
        [TestMethod]
        public void Test_GetMethod_ReturnsExpectedResult()
        {
            // Arrange
            var controller = new CounterController();

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result); // Replace with actual assertions based on the method's behavior
        }
    }
}
