using LpApi_20210506.Common;
using LpApi_20210506.DataAccess;
using LpApi_20210506.Interfaces;
using LpApi_20210506.Main.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LpApi_20210506.Main.Tests.Controllers
{
    [TestClass()]
    public class CustomersApiControllerTests
    {
        [TestMethod()]
        public void RegisterCustomerTest()
        {
            // Arrange
            var dbRepo = new Mock<IDatabaseRepository>();
            var logger = new Mock<ILogger<CustomersApiController>>();
            var config = new Mock<IOptions<GrandParadeConfiguration>>();
            config.Setup(x => x.Value).Returns(new GrandParadeConfiguration());
            var controller = new CustomersApiController(logger.Object, dbRepo.Object, config.Object);
            dbRepo.Setup(x =>
                x.GetCustomer(It.IsAny<string>())).Returns<Customer>(null);
            dbRepo.Setup(x =>
                x.SaveCustomer(It.IsAny<Customer>())).Returns(true);

            // Act
            var response = controller.RegisterCustomer("firstname", "lastname", "street", "1", "zipcode", "personalnumber",
                "favoriteteam", CustomerSourceEnum.MrGreen);
   
            // Assert
            Assert.IsTrue(response is OkObjectResult);
        }
    }
}
