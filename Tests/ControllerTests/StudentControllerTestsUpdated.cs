using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Services;
using StudyBuddy.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.Tests.ControllerTests
{
    public class StudentControllerTests
    {
        [Fact]
        public async Task GetStudents_ReturnsOkObjectResult_WithListOfStudents()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            mockService.Setup(service => service.GetAllStudentsAsync()).ReturnsAsync(new List<StudentDto>());
            var controller = new StudentController(mockService.Object);

            // Act
            var result = await controller.GetStudents();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<StudentDto>>(actionResult.Value);
        }

        [Fact]
        public async Task GetStudent_ReturnsOkObjectResult_WhenStudentExists()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            int testStudentId = 1;
            mockService.Setup(service => service.GetStudentByIdAsync(testStudentId)).ReturnsAsync(new StudentDto { StudentId = testStudentId });
            var controller = new StudentController(mockService.Object);

            // Act
            var result = await controller.GetStudent(testStudentId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedStudent = Assert.IsType<StudentDto>(actionResult.Value);
            Assert.Equal(testStudentId, returnedStudent.StudentId);
        }

        [Fact]
        public async Task GetStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            int testStudentId = 999; // Assuming this ID does not exist
            mockService.Setup(service => service.GetStudentByIdAsync(testStudentId)).ReturnsAsync((StudentDto)null);
            var controller = new StudentController(mockService.Object);

            // Act
            var result = await controller.GetStudent(testStudentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Additional Unit Tests

        [Fact]
        public async Task CreateStudent_ReturnsCreatedAtActionResult_WhenStudentIsCreated()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var newStudent = new StudentDto { StudentId = 2 };
            mockService.Setup(service => service.CreateStudentAsync(It.IsAny<StudentDto>())).ReturnsAsync(newStudent);
            var controller = new StudentController(mockService.Object);

            // Act
            var result = await controller.CreateStudent(newStudent);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdStudent = Assert.IsType<StudentDto>(actionResult.Value);
            Assert.Equal(newStudent.StudentId, createdStudent.StudentId);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsNoContent_WhenStudentIsDeleted()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            int testStudentId = 1;
            mockService.Setup(service => service.DeleteStudentAsync(testStudentId)).ReturnsAsync(true);
            var controller = new StudentController(mockService.Object);

            // Act
            var result = await controller.DeleteStudent(testStudentId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            int testStudentId = 999; // Assuming this ID does not exist
            mockService.Setup(service => service.DeleteStudentAsync(testStudentId)).ReturnsAsync(false);
            var controller = new StudentController(mockService.Object);

            // Act
            var result = await controller.DeleteStudent(testStudentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
