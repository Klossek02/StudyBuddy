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

        // Additional Unit Tests

        [Fact]
        public async Task Login_ReturnsOkResult_WhenLoginIsSuccessful()
        {
            // Arrange
            var mockSignInManager = new Mock<SignInManager<IdentityUser>>(Mock.Of<UserManager<IdentityUser>>(), null, null, null, null, null, null, null);
            mockSignInManager.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Success);
            var controller = new AuthController(mockSignInManager.Object, null);

            // Act
            var result = await controller.Login(new LoginModel { Email = "user@example.com", Password = "SecurePassword!" });

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenLoginFails()
        {
            // Arrange
            var mockSignInManager = new Mock<SignInManager<IdentityUser>>(Mock.Of<UserManager<IdentityUser>>(), null, null, null, null, null, null, null);
            mockSignInManager.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Failed);
            var controller = new AuthController(mockSignInManager.Object, null);

            // Act
            var result = await controller.Login(new LoginModel { Email = "user@example.com", Password = "WrongPassword" });

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
