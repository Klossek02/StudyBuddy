using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Services;
using StudyBuddy.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.Unit_Tests
{
    public class AdminUnitTests
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
            Assert.IsType<ActionResult<AdminDto>>(result);
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
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
            var actionResult = Assert.IsType<ActionResult<AdminDto>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAdmins_ReturnsOkObjectResult_WithListOfAdmins()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var adminsList = new List<AdminDto>
            {
                new AdminDto { AdminId = 1 },
                new AdminDto { AdminId = 2 }
            };
            mockService.Setup(service => service.GetAllAdminsAsync()).ReturnsAsync(adminsList);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.GetAdmins();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAdmins = Assert.IsType<List<AdminDto>>(actionResult.Value);
            Assert.Equal(adminsList.Count, returnedAdmins.Count);
        }

        [Fact]
        public async Task GetAdmins_ReturnsOkObjectResult_WithEmptyList()
        {
            // Arrange
            var mockService = new Mock<IAdminService>();
            var adminsList = new List<AdminDto>(); // Empty list
            mockService.Setup(service => service.GetAllAdminsAsync()).ReturnsAsync(adminsList);
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.GetAdmins();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAdmins = Assert.IsType<List<AdminDto>>(actionResult.Value);
            Assert.Empty(returnedAdmins);
        }
    }
}
