using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Models;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.Tests.ControllerTests
{
    public class ExtendedAuthControllerTests
    {
        [Fact]
        public async Task Register_ReturnsOkResult_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var mockAuthService = new Mock<IAuthService>();
            var controller = new AuthController(mockUserManager.Object, mockAuthService.Object);

            // Act
            var result = await controller.Register(new RegisterModel { Email = "newuser@example.com", Password = "NewSecurePassword!" });

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
            var mockAuthService = new Mock<IAuthService>();
            var controller = new AuthController(mockUserManager.Object, mockAuthService.Object);

            // Act
            var result = await controller.Register(new RegisterModel { Email = "newuser@example.com", Password = "NewSecurePassword!" });

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}