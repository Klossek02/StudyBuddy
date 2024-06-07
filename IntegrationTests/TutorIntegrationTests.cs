using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using StudyBuddy.DTO;
using StudyBuddy;

public class TutorIntegrationTests : IClassFixture<WebApplicationFactory<StudyBuddy.Startup>>
{
    private readonly HttpClient _client;

    public TutorIntegrationTests(WebApplicationFactory<StudyBuddy.Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTutor_ReturnsCreatedAtActionResult_WhenTutorIsCreated()
    {
        // Arrange
        var tutorDto = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "Password123",
            ExpertiseArea = "Math"
        };
        var content = new StringContent(JsonConvert.SerializeObject(tutorDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/Tutor", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedTutor = JsonConvert.DeserializeObject<Tutor>(responseString);
        Assert.Equal(tutorDto.FirstName, returnedTutor.FirstName);
        Assert.Equal(tutorDto.LastName, returnedTutor.LastName);
        Assert.Equal(tutorDto.Email, returnedTutor.Email);
    }

    [Fact]
    public async Task GetTutorById_ReturnsOkObjectResult_WhenTutorExists()
    {
        // Act
        var response = await _client.GetAsync("/api/Tutor/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedTutor = JsonConvert.DeserializeObject<Tutor>(responseString);
        Assert.Equal(1, returnedTutor.TutorId);
    }

    [Fact]
    public async Task GetTutorById_ReturnsNotFound_WhenTutorDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/Tutor/999");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTutor_ReturnsOkObjectResult_WhenTutorIsUpdated()
    {
        // Arrange
        var tutorDto = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            ExpertiseArea = "Math"
        };
        var content = new StringContent(JsonConvert.SerializeObject(tutorDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/Tutor/1", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var returnedTutor = JsonConvert.DeserializeObject<Tutor>(responseString);
        Assert.Equal(tutorDto.FirstName, returnedTutor.FirstName);
        Assert.Equal(tutorDto.LastName, returnedTutor.LastName);
        Assert.Equal(tutorDto.Email, returnedTutor.Email);
    }

    [Fact]
    public async Task DeleteTutor_ReturnsNoContent_WhenTutorIsDeleted()
    {
        // Act
        var response = await _client.DeleteAsync("/api/Tutor/1");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
}
