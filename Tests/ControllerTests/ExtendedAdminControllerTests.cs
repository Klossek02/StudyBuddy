using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Services;
using StudyBuddy.DTO;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.Tests.ControllerTests
{
    public class ExtendedAdminControllerTests
    {
        [Fact]
        public async Task GetAdmin_ReturnsOkObjectResult_WhenAdminExists()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            int testAdminId = 1;
            mockService.Setup(service => service.GetAdminByIdAsync(testAdminId)).ReturnsAsync(new AdminDto { AdminId = testAdminId });
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.GetAdmin(testAdminId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedAdmin = Assert.IsType<AdminDto>(actionResult.Value);
            Assert.Equal(testAdminId, returnedAdmin.AdminId);
        }

        [Fact]
        public async Task GetAdmin_ReturnsNotFound_WhenAdminDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            int testAdminId = 999; // Assuming this ID does not exist
            mockService.Setup(service => service.GetAdminByIdAsync(testAdminId)).ReturnsAsync((AdminDto)null);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.GetAdmin(testAdminId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}