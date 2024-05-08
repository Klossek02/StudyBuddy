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
    public class TutorUnitTests
    {
        [Fact]
        public async Task GetTutors_ReturnsOkObjectResult_WithListOfTutors()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            mockService.Setup(service => service.GetAllTutorsAsync()).ReturnsAsync(new List<TutorDto>());
            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.GetTutors();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<TutorDto>>(actionResult.Value);
        }

        [Fact]
        public async Task GetTutor_ReturnsOkObjectResult_WhenTutorExists()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            int testTutorId = 1;
            mockService.Setup(service => service.GetTutorByIdAsync(testTutorId)).ReturnsAsync(new TutorDto { TutorId = testTutorId });
            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.GetTutor(testTutorId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedTutor = Assert.IsType<TutorDto>(actionResult.Value);
            Assert.Equal(testTutorId, returnedTutor.TutorId);
        }

        [Fact]
        public async Task GetTutor_ReturnsNotFound_WhenTutorDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            int testTutorId = 999; // Assuming this ID does not exist
            mockService.Setup(service => service.GetTutorByIdAsync(testTutorId)).ReturnsAsync((TutorDto)null);
            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.GetTutor(testTutorId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
