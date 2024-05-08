using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Models;
using System;
using System.Threading.Tasks;
using Xunit;


namespace StudyBuddy.Unit_Tests
{
    public class AuthUnitTests
    {
        private readonly Mock<UserManager<IdentityUser>> mockUserManager;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<ApplicationDbContext> mockDbContext;
        private readonly PasswordHasher<IdentityUser> passwordHasher;


        public AuthUnitTests()
        {
            mockUserManager = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            mockConfiguration = new Mock<IConfiguration>();
            mockDbContext = new Mock<ApplicationDbContext>();
            passwordHasher = new PasswordHasher<IdentityUser>();

            // setup configuration with specific values
            mockConfiguration.Setup(c => c["JwtSettings:SecretKey"]).Returns("VerySecretKey");
            mockConfiguration.Setup(c => c["JwtSettings:Issuer"]).Returns("Issuer");
            mockConfiguration.Setup(c => c["JwtSettings:Audience"]).Returns("Audience");
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var controller = new AuthController(mockUserManager.Object, mockConfiguration.Object, passwordHasher, mockDbContext.Object);
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Register(new RegisterModel { Email = "newuser@example.com", Password = "NewSecurePassword!" });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created successfully", okResult.Value);
        }
        /*

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var controller = new AuthController(mockUserManager.Object, mockConfiguration.Object, passwordHasher, mockDbContext.Object);
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await controller.Register(new RegisterModel { Email = "newuser@example.com", Password = "NewSecurePassword!" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        */

        /*

        [Fact]
        public async Task Register_CreatesValidJwtToken_WhenRegistrationIsSuccessful()
        {
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            var controller = new AuthController(mockUserManager.Object, mockConfiguration.Object, passwordHasher, null);

            // Act
            var result = await controller.Register(new RegisterModel { Email = "test@example.com", Password = "TestPassword123!" }) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var authResponse = Assert.IsType<AuthResponse>(result.Value);
            Assert.NotNull(authResponse.AccessToken);
            // Additional assertions to validate the contents of the JWT token as needed
        }
        */
    }
}
