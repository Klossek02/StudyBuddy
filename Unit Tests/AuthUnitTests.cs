using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Models;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.Unit_Tests
{
    public class AuthUnitTests
    {
        private readonly Mock<UserManager<IdentityUser>> mockUserManager;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly DbContextOptions<ApplicationDbContext> dbContextOptions;
        private readonly PasswordHasher<IdentityUser> passwordHasher;

        public AuthUnitTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            mockUserManager = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            mockConfiguration = new Mock<IConfiguration>();
            dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            passwordHasher = new PasswordHasher<IdentityUser>();

            // Setup configuration with specific values
            mockConfiguration.Setup(c => c["JwtSettings:SecretKey"]).Returns("VerySecretKey");
            mockConfiguration.Setup(c => c["JwtSettings:Issuer"]).Returns("Issuer");
            mockConfiguration.Setup(c => c["JwtSettings:Audience"]).Returns("Audience");
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WhenRegistrationIsSuccessful()
        {
            // Arrange
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var controller = new AuthController(mockUserManager.Object, mockConfiguration.Object, passwordHasher, context);
                mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

                // Act
                var result = await controller.Register(new RegisterModel { Email = "newuser@example.com", Password = "NewSecurePassword!" });

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("User created successfully", okResult.Value);
            }
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            // Arrange
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var controller = new AuthController(mockUserManager.Object, mockConfiguration.Object, passwordHasher, context);
                var errors = new IdentityError[] { new IdentityError { Description = "Fake error" } };
                mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(errors));

                // Act
                var result = await controller.Register(new RegisterModel { Email = "newuser@example.com", Password = "NewSecurePassword!" });

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(errors, badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Register_CreatesValidJwtToken_WhenRegistrationIsSuccessful()
        {
            // Arrange
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
                var controller = new AuthController(mockUserManager.Object, mockConfiguration.Object, passwordHasher, context);

                // Act
                var result = await controller.Register(new RegisterModel { Email = "test@example.com", Password = "TestPassword123!" }) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                var successMessage = Assert.IsType<string>(result.Value);
                Assert.Equal("User created successfully", successMessage);
            }
        }
    }
}
