using Microsoft.EntityFrameworkCore;
using StudyBuddy.DTO;
using StudyBuddy.Models;
using StudyBuddy.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace StudyBuddy.UnitTests
{
    public class LessonServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonService _service;

        public LessonServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "StudyBuddyTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new LessonService(_context);
        }

        [Fact]
        public async Task CreateLessonAsync_CreatesAndReturnsLesson()
        {
            // Arrange
            var lessonDto = new CreateLessonDto { StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now };

            // Act
            var result = await _service.CreateLessonAsync(lessonDto);

            // Assert
            Assert.Equal(lessonDto.StudentId, result.StudentId);
            Assert.Equal(lessonDto.TutorId, result.TutorId);
            Assert.Equal(lessonDto.Subject, result.Subject);
            Assert.Equal(LessonState.Requested, result.State);
        }

        [Fact]
        public async Task GetLessonByIdAsync_ReturnsLesson_WhenLessonExists()
        {
            // Arrange
            var lesson = new Lesson { LessonId = 0, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Requested };
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            var lessonId = lesson.LessonId;

            // Act
            var result = await _service.GetLessonByIdAsync(lessonId);

            // Assert
            Assert.Equal(lessonId, result.LessonId);
        }

        [Fact]
        public async Task GetLessonByIdAsync_ReturnsNull_WhenLessonDoesNotExist()
        {
            // Arrange
            var lessonId = 999;

            // Act
            var result = await _service.GetLessonByIdAsync(lessonId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AcceptLessonAsync_UpdatesLessonStateToAccepted()
        {
            // Arrange
            var lesson = new Lesson { LessonId = 0, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Requested };
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            var lessonId = lesson.LessonId;

            // Act
            var result = await _service.AcceptLessonAsync(lessonId);

            // Assert
            Assert.Equal(LessonState.Accepted, result.State);
        }

        [Fact]
        public async Task RejectLessonAsync_UpdatesLessonStateToRejected()
        {
            // Arrange
            var lesson = new Lesson { LessonId = 0, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now, State = LessonState.Requested };
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            var lessonId = lesson.LessonId;

            // Act
            var result = await _service.RejectLessonAsync(lessonId);

            // Assert
            Assert.Equal(LessonState.Rejected, result.State);
        }

        [Fact]
        public async Task UpdateLessonAsync_UpdatesLessonAndReturnsUpdatedLesson()
        {
            // Arrange
            var lesson = new Lesson { LessonId = 0, StudentId = 1, TutorId = 1, Subject = "Math", LessonDate = DateTime.Now.AddDays(-1), State = LessonState.Requested };
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            var lessonId = lesson.LessonId;

            var lessonDto = new UpdateLessonDto { LessonDate = DateTime.Now, State = LessonState.Accepted };

            // Act
            var result = await _service.UpdateLessonAsync(lessonId, lessonDto);

            // Assert
            Assert.Equal(lessonDto.LessonDate, result.LessonDate);
            Assert.Equal(lessonDto.State, result.State);
        }
    }
}
