using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using StudyBuddy.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.UnitTests
{
    public class LessonsControllerTests
    {
        private readonly Mock<ILessonService> _mockLessonService;
        private readonly LessonsController _controller;

        public LessonsControllerTests()
        {
            _mockLessonService = new Mock<ILessonService>();
            _controller = new LessonsController(_mockLessonService.Object);
        }

        [Fact]
        public async Task RequestLesson_ReturnsCreatedAtActionResult_WhenLessonIsCreated()
        {
            // Arrange
            var lessonDto = new CreateLessonDto { StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now };
            var lesson = new Lesson { LessonId = 1, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Requested };

            _mockLessonService.Setup(service => service.CreateLessonAsync(lessonDto)).ReturnsAsync(lesson);

            // Act
            var result = await _controller.RequestLesson(lessonDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedLesson = Assert.IsType<Lesson>(actionResult.Value);
            Assert.Equal(lesson.LessonId, returnedLesson.LessonId);
        }

        [Fact]
        public async Task GetLessonById_ReturnsOkObjectResult_WhenLessonExists()
        {
            // Arrange
            var lessonId = 1;
            var lesson = new Lesson { LessonId = lessonId, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Requested };

            _mockLessonService.Setup(service => service.GetLessonByIdAsync(lessonId)).ReturnsAsync(lesson);

            // Act
            var result = await _controller.GetLessonById(lessonId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedLesson = Assert.IsType<Lesson>(actionResult.Value);
            Assert.Equal(lessonId, returnedLesson.LessonId);
        }

        [Fact]
        public async Task GetLessonById_ReturnsNotFound_WhenLessonDoesNotExist()
        {
            // Arrange
            var lessonId = 999;

            _mockLessonService.Setup(service => service.GetLessonByIdAsync(lessonId)).ReturnsAsync((Lesson)null);

            // Act
            var result = await _controller.GetLessonById(lessonId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AcceptLesson_ReturnsOkObjectResult_WhenLessonIsAccepted()
        {
            // Arrange
            var lessonId = 1;
            var lesson = new Lesson { LessonId = lessonId, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Accepted };

            _mockLessonService.Setup(service => service.AcceptLessonAsync(lessonId)).ReturnsAsync(lesson);

            // Act
            var result = await _controller.AcceptLesson(lessonId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedLesson = Assert.IsType<Lesson>(actionResult.Value);
            Assert.Equal(LessonState.Accepted, returnedLesson.State);
        }

        [Fact]
        public async Task RejectLesson_ReturnsOkObjectResult_WhenLessonIsRejected()
        {
            // Arrange
            var lessonId = 1;
            var lesson = new Lesson { LessonId = lessonId, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Rejected };

            _mockLessonService.Setup(service => service.RejectLessonAsync(lessonId)).ReturnsAsync(lesson);

            // Act
            var result = await _controller.RejectLesson(lessonId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedLesson = Assert.IsType<Lesson>(actionResult.Value);
            Assert.Equal(LessonState.Rejected, returnedLesson.State);
        }

        [Fact]
        public async Task UpdateLesson_ReturnsOkObjectResult_WhenLessonIsUpdated()
        {
            // Arrange
            var lessonId = 1;
            var lessonDto = new UpdateLessonDto { LessonDate = DateTime.Now, State = LessonState.Accepted };
            var lesson = new Lesson { LessonId = lessonId, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Accepted };

            _mockLessonService.Setup(service => service.UpdateLessonAsync(lessonId, lessonDto)).ReturnsAsync(lesson);

            // Act
            var result = await _controller.UpdateLesson(lessonId, lessonDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedLesson = Assert.IsType<Lesson>(actionResult.Value);
            Assert.Equal(LessonState.Accepted, returnedLesson.State);
        }
    }
}