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

        // Additional Unit Tests

        [Fact]
        public async Task CreateAdmin_ReturnsCreatedAtActionResult_WhenAdminIsCreated()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var newAdmin = new AdminDto { AdminId = 2 };
            mockService.Setup(service => service.CreateAdminAsync(It.IsAny<AdminDto>())).ReturnsAsync(newAdmin);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.CreateAdmin(newAdmin);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdAdmin = Assert.IsType<AdminDto>(actionResult.Value);
            Assert.Equal(newAdmin.AdminId, createdAdmin.AdminId);
        }

        [Fact]
        public async Task UpdateAdmin_ReturnsNoContent_WhenAdminIsUpdated()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var updateAdmin = new AdminDto { AdminId = 1 };
            mockService.Setup(service => service.UpdateAdminAsync(updateAdmin)).ReturnsAsync(true);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.UpdateAdmin(updateAdmin.AdminId, updateAdmin);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAdmin_ReturnsNotFound_WhenAdminDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var updateAdmin = new AdminDto { AdminId = 999 }; // Assuming this ID does not exist
            mockService.Setup(service => service.UpdateAdminAsync(updateAdmin)).ReturnsAsync(false);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.UpdateAdmin(updateAdmin.AdminId, updateAdmin);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAdmin_ReturnsNoContent_WhenAdminIsDeleted()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            int testAdminId = 1;
            mockService.Setup(service => service.DeleteAdminAsync(testAdminId)).ReturnsAsync(true);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.DeleteAdmin(testAdminId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAdmin_ReturnsNotFound_WhenAdminDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            int testAdminId = 999; // Assuming this ID does not exist
            mockService.Setup(service => service.DeleteAdminAsync(testAdminId)).ReturnsAsync(false);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.DeleteAdmin(testAdminId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
