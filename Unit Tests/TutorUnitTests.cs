using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Services;
using StudyBuddy.DTO;
using StudyBuddy.Models;
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
        public async Task GetTutors_ReturnsOkObjectResult_WithEmptyList()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            mockService.Setup(service => service.GetAllTutorsAsync()).ReturnsAsync(new List<TutorDto>());
            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.GetTutors();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedTutors = Assert.IsType<List<TutorDto>>(actionResult.Value);
            Assert.Empty(returnedTutors);
        }


        [Fact]
        public async Task CreateTutor_ReturnsBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            var tutorModel = new TutorCreateModel
            {
                FirstName = "",
                LastName = "",
                Email = "invalid-email",
                Password = "short"
            };
            var controller = new TutorController(mockService.Object);
            controller.ModelState.AddModelError("FirstName", "Required");
            controller.ModelState.AddModelError("LastName", "Required");
            controller.ModelState.AddModelError("Email", "Invalid email format");
            controller.ModelState.AddModelError("Password", "Too short");

            // Act
            var result = await controller.CreateTutor(tutorModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTutor_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            var tutorModel = new TutorCreateModel
            {
                FirstName = "Carrie",
                LastName = "Fisher",
                Email = "carrie@fisher.com",
                ExpertiseArea = "Math",
                Password = "Password123"
            };
            int testTutorId = 1;
            var existingTutor = new TutorDto
            {
                TutorId = testTutorId,
                FirstName = "Leia",
                LastName = "Organa",
                Email = "leia.organa@example.com",
                ExpertiseArea = "Math"
            };

            mockService.Setup(service => service.GetTutorByIdAsync(testTutorId)).ReturnsAsync(existingTutor);
            mockService.Setup(service => service.UpdateTutorAsync(testTutorId, tutorModel)).ReturnsAsync(true);

            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.UpdateTutor(testTutorId, tutorModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTutor_ReturnsNotFound_WhenTutorDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            var tutorModel = new TutorCreateModel
            {
                FirstName = "Han",
                LastName = "Solo",
                Email = "han@solo.com",
                ExpertiseArea = "Math",
                Password = "eigh6#d"
            };
            int testTutorId = 888; // We have to assume this id doesn't exist
            mockService.Setup(service => service.UpdateTutorAsync(testTutorId, tutorModel)).ReturnsAsync(false);
            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.UpdateTutor(testTutorId, tutorModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateTutor_ReturnsBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            var tutorModel = new TutorCreateModel
            {
                FirstName = "",
                LastName = "",
                Email = "invalid-email",
                Password = "short"
            };
            int testTutorId = 1;
            var controller = new TutorController(mockService.Object);
            controller.ModelState.AddModelError("FirstName", "Required");
            controller.ModelState.AddModelError("LastName", "Required");
            controller.ModelState.AddModelError("Email", "Invalid email format");
            controller.ModelState.AddModelError("Password", "Too short");

            // Act
            var result = await controller.UpdateTutor(testTutorId, tutorModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTutor_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            int testTutorId = 1;
            mockService.Setup(service => service.DeleteTutorAsync(testTutorId)).ReturnsAsync(true);
            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.DeleteTutor(testTutorId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTutor_ReturnsNotFound_WhenTutorDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITutorService>();
            int testTutorId = 777; // We have to assume this id doesn't exist
            mockService.Setup(service => service.DeleteTutorAsync(testTutorId)).ReturnsAsync(false);
            var controller = new TutorController(mockService.Object);

            // Act
            var result = await controller.DeleteTutor(testTutorId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
