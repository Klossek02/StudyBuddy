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
    }
}