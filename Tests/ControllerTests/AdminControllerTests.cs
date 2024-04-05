using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.Tests.ControllerTests
{
    public class AdminControllerTests
    {
        [Fact]
        public async Task GetAdmins_ReturnsOkObjectResult_WithListOfAdmins()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            mockService.Setup(service => service.GetAllAdminsAsync()).ReturnsAsync(new List<AdminDto>());
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.GetAdmins();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<AdminDto>>(actionResult.Value);
        }
    }
}