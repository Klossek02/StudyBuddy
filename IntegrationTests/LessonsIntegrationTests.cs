using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using StudyBuddy.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using StudyBuddy.DTO;
using StudyBuddy;

public class LessonsIntegrationTests : IClassFixture<WebApplicationFactory<StudyBuddy.Startup>>
{
    private readonly HttpClient _client;

    public LessonsIntegrationTests(WebApplicationFactory<StudyBuddy.Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RequestLesson_ReturnsCreatedAtActionResult_WhenLessonIsCreated()
    {
        // Arrange
        var lessonDto = new
        {
            StudentId = 1,
            TutorId = 1,
            Subject = "Math",
            LessonDate = DateTime.Now
        };
        var content = new StringContent(JsonConvert.SerializeObject(lessonDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Lessons/request", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedLesson = JsonConvert.DeserializeObject<Lesson>(responseString);
        Assert.Equal(lessonDto.StudentId, returnedLesson.StudentId);
        Assert.Equal(lessonDto.TutorId, returnedLesson.TutorId);
        Assert.Equal(lessonDto.Subject, returnedLesson.Subject);
    }

    [Fact]
    public async Task GetLessonById_ReturnsOkObjectResult_WhenLessonExists()
    {
        // Act
        var response = await _client.GetAsync("/api/Lessons/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedLesson = JsonConvert.DeserializeObject<Lesson>(responseString);
        Assert.Equal(1, returnedLesson.LessonId);
    }

    [Fact]
    public async Task GetLessonById_ReturnsNotFound_WhenLessonDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/Lessons/999");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AcceptLesson_ReturnsOkObjectResult_WhenLessonIsAccepted()
    {
        // Act
        var response = await _client.PutAsync("/api/Lessons/1/accept", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedLesson = JsonConvert.DeserializeObject<Lesson>(responseString);
        Assert.Equal(LessonState.Accepted, returnedLesson.State);
    }

    [Fact]
    public async Task RejectLesson_ReturnsOkObjectResult_WhenLessonIsRejected()
    {
        // Act
        var response = await _client.PutAsync("/api/Lessons/1/reject", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedLesson = JsonConvert.DeserializeObject<Lesson>(responseString);
        Assert.Equal(LessonState.Rejected, returnedLesson.State);
    }

    [Fact]
    public async Task UpdateLesson_ReturnsOkObjectResult_WhenLessonIsUpdated()
    {
        // Arrange
        var lessonDto = new
        {
            LessonDate = DateTime.Now,
            State = LessonState.Accepted
        };
        var content = new StringContent(JsonConvert.SerializeObject(lessonDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/Lessons/1", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedLesson = JsonConvert.DeserializeObject<Lesson>(responseString);
        Assert.Equal(LessonState.Accepted, returnedLesson.State);
    }
}
