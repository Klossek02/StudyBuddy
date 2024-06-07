using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using StudyBuddy.DTO;
using StudyBuddy;

public class StudentIntegrationTests : IClassFixture<WebApplicationFactory<StudyBuddy.Startup>>
{
    private readonly HttpClient _client;

    public StudentIntegrationTests(WebApplicationFactory<StudyBuddy.Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateStudent_ReturnsCreatedAtActionResult_WhenStudentIsCreated()
    {
        // Arrange
        var studentDto = new
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Password = "Password123"
        };
        var content = new StringContent(JsonConvert.SerializeObject(studentDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Student", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedStudent = JsonConvert.DeserializeObject<Student>(responseString);
        Assert.Equal(studentDto.FirstName, returnedStudent.FirstName);
        Assert.Equal(studentDto.LastName, returnedStudent.LastName);
        Assert.Equal(studentDto.Email, returnedStudent.Email);
    }

    [Fact]
    public async Task GetStudentById_ReturnsOkObjectResult_WhenStudentExists()
    {
        // Act
        var response = await _client.GetAsync("/api/Student/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedStudent = JsonConvert.DeserializeObject<Student>(responseString);
        Assert.Equal(1, returnedStudent.StudentId);
    }

    [Fact]
    public async Task GetStudentById_ReturnsNotFound_WhenStudentDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/Student/999");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsOkObjectResult_WhenStudentIsUpdated()
    {
        // Arrange
        var studentDto = new
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com"
        };
        var content = new StringContent(JsonConvert.SerializeObject(studentDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/Student/1", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedStudent = JsonConvert.DeserializeObject<Student>(responseString);
        Assert.Equal(studentDto.FirstName, returnedStudent.FirstName);
        Assert.Equal(studentDto.LastName, returnedStudent.LastName);
        Assert.Equal(studentDto.Email, returnedStudent.Email);
    }

    [Fact]
    public async Task DeleteStudent_ReturnsNoContent_WhenStudentIsDeleted()
    {
        // Act
        var response = await _client.DeleteAsync("/api/Student/1");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
}
