using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Projects;
using Xunit;

namespace TaskManagement.Tests.Integration
{
    public class ProjectsIntegrationTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProjectsIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProject_AddTask_DeleteProject_ShouldSucceed()
        {
            var project = new
            {
                name = "Integration Test Project",
                description = "Testing"
            };

            var projectResponse =
                await _client.PostAsJsonAsync("/api/Projects", project);

            projectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var createdProject =
    await projectResponse.Content.ReadFromJsonAsync<ProjectDto>();

            int projectId = createdProject!.Id;

            var task = new
            {
                projectId = projectId,
                title = "Task 1",
                description = "Test Task",
                status = 1,
                priority = 2,
                dueDate = DateTime.UtcNow.AddDays(2)
            };

            var taskResponse =
                await _client.PostAsJsonAsync($"/api/Projects/{projectId}/tasks", task);

            taskResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var deleteResponse =
                await _client.DeleteAsync($"/api/Projects/{projectId}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}