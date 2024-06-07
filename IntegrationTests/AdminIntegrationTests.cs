using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using StudyBuddy.DTO;
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
    public class AdminIntegrationTests : IClassFixture<WebApplicationFactory<StudyBuddy.Startup>>
    {
        private readonly HttpClient _client;

        public AdminIntegrationTests(WebApplicationFactory<StudyBuddy.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAdmins_ReturnsOkObjectResult_WithListOfAdmins()
        {
            // Act
            var response = await _client.GetAsync("/api/Admin");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var admins = JsonConvert.DeserializeObject<List<AdminDto>>(responseString);
            Assert.NotEmpty(admins);
        }

        [Fact]
        public async Task GetAdminById_ReturnsOkObjectResult_WhenAdminExists()
        {
            // Act
            var response = await _client.GetAsync("/api/Admin/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var admin = JsonConvert.DeserializeObject<AdminDto>(responseString);
            Assert.Equal(1, admin.Id);
        }

        [Fact]
        public async Task GetAdminById_ReturnsNotFound_WhenAdminDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/Admin/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
