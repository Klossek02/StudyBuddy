using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Services;
using StudyBuddy.Models;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.Tests.ControllerTests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ReturnsOkObjectResult_WhenCredentialsAreValid()
        {
            // Arrange
            var mockService = new Mock<IAuthService>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var controller = new AuthController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.Login(new LoginModel { Email = "valid@example.com", Password = "ValidPassword" });

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenCredentialsAreInvalid()
        {
            // Arrange
            var mockService = new Mock<IAuthService>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var controller = new AuthController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.Login(new LoginModel { Email = "invalid@example.com", Password = "InvalidPassword" });

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}