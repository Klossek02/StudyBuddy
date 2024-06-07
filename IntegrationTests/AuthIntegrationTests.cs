using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using StudyBuddy.DTO;
using StudyBuddy;

namespace StudyBuddy.IntegrationTests
{
    public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<StudyBuddy.Startup>>
    {
        private readonly HttpClient _client;

        public AuthIntegrationTests(WebApplicationFactory<StudyBuddy.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WhenUserIsRegistered()
        {
            // Arrange
            var registerDto = new
            {
                Email = "newuser@example.com",
                Password = "Password123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Auth/register", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User created successfully", responseString);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenUserIsAuthenticated()
        {
            // Arrange
            var loginDto = new
            {
                Email = "existinguser@example.com",
                Password = "Password123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Auth/login", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseString);
            Assert.NotNull(authResponse.AccessToken);
        }

        [Fact]
        public async Task RefreshToken_ReturnsOkResult_WhenRefreshTokenIsValid()
        {
            // Arrange
            var refreshTokenRequest = new
            {
                Token = "valid-refresh-token"
            };
            var content = new StringContent(JsonConvert.SerializeObject(refreshTokenRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Auth/refresh-token", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseString);
            Assert.NotNull(authResponse.AccessToken);
        }
    }
}
