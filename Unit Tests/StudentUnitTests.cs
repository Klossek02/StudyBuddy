using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudyBuddy.Controllers;
using StudyBuddy.Services;
using StudyBuddy.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using StudyBuddy.Models;

namespace StudyBuddy.Unit_Tests
{
    public class StudentUnitTests
    {
        private Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            return mockUserManager;
        }

        [Fact]
        public async Task GetStudents_ReturnsOkObjectResult_WithListOfStudents()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            mockService.Setup(service => service.GetAllStudentsAsync()).ReturnsAsync(new List<StudentDto>());
            var controller = new StudentController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.GetStudents();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<StudentDto>>(actionResult.Value);
        }

        [Fact]
        public async Task GetStudents_ReturnsOkObjectResult_WithEmptyList()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            mockService.Setup(service => service.GetAllStudentsAsync()).ReturnsAsync(new List<StudentDto>());
            var controller = new StudentController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.GetStudents();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedStudents = Assert.IsType<List<StudentDto>>(actionResult.Value);
            Assert.Empty(returnedStudents);
        }

        [Fact]
        public async Task GetStudent_ReturnsOkObjectResult_WhenStudentExists()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            int testStudentId = 1;
            mockService.Setup(service => service.GetStudentByIdAsync(testStudentId)).ReturnsAsync(new StudentDto { StudentId = testStudentId });
            var controller = new StudentController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.GetStudent(testStudentId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedStudent = Assert.IsType<StudentDto>(actionResult.Value);
            Assert.Equal(testStudentId, returnedStudent.StudentId);
        }

        [Fact]
        public async Task GetStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            int testStudentId = 111; // assume this id doesn't exist
            mockService.Setup(service => service.GetStudentByIdAsync(testStudentId)).ReturnsAsync((StudentDto)null);
            var controller = new StudentController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.GetStudent(testStudentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateStudent_ReturnsBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            var studentModel = new StudentCreateModel
            {
                FirstName = "",
                LastName = "",
                Email = "invalid-email",
                Password = "short"
            };
            var controller = new StudentController(mockService.Object, mockUserManager.Object);
            controller.ModelState.AddModelError("FirstName", "Required");
            controller.ModelState.AddModelError("LastName", "Required");
            controller.ModelState.AddModelError("Email", "Invalid email format");
            controller.ModelState.AddModelError("Password", "Too short");

            // Act
            var result = await controller.CreateStudent(studentModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateStudent_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            var studentModel = new StudentCreateModel
            {
                FirstName = "Terry",
                LastName = "Pratchett",
                Email = "terry.pratchett@gmail.com",
                Password = "dIsKwOrLd2#"
            };
            int testStudentId = 1;
            mockService.Setup(service => service.UpdateStudentAsync(testStudentId, studentModel)).ReturnsAsync(true);
            var controller = new StudentController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.UpdateStudent(testStudentId, studentModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            var studentModel = new StudentCreateModel
            {
                FirstName = "Stephen",
                LastName = "King",
                Email = "stephen.king@gmail.com",
                Password = "CaRRiE@76"
            };
            int testStudentId = 222;  // assume this id doesn't exist
            mockService.Setup(service => service.UpdateStudentAsync(testStudentId, studentModel)).ReturnsAsync(false);
            var controller = new StudentController(mockService.Object, mockUserManager.Object);

            // Act
            var result = await controller.UpdateStudent(testStudentId, studentModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateStudent_ReturnsBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var mockService = new Mock<IStudentService>();
            var mockUserManager = GetMockUserManager();
            var studentModel = new StudentCreateModel
            {
                FirstName = "",
                LastName = "",
                Email = "invalid-email",
                Password = "short"
            };
            int testStudentId = 1;
            var controller = new StudentController(mockService.Object, mockUserManager.Object);
            controller.ModelState.AddModelError("FirstName", "Required");
            controller.ModelState.AddModelError("LastName", "Required");
            controller.ModelState.AddModelError("Email", "Invalid email format");
            controller.ModelState.AddModelError("Password", "Too short");

            // Act
            var result = await controller.UpdateStudent(testStudentId, studentModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
