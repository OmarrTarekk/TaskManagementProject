using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.DTOs.Tasks;
using Xunit;

namespace TaskManagement.Tests.Integration;

public class TasksIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TasksIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FilterTasks_ByStatusAndPriority_ShouldReturnCorrectTask()
    {
        var project = new
        {
            name = "Filter Test Project",
            description = "Testing Filter"
        };

        var projectResponse =
            await _client.PostAsJsonAsync("/api/Projects", project);

        projectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdProject =
            await projectResponse.Content.ReadFromJsonAsync<ProjectDto>();

        int projectId = createdProject!.Id;

        var createTask1Response = await _client.PostAsJsonAsync(
            $"/api/Projects/{projectId}/tasks",
            new
            {
                projectId,
                title = "Task Todo High",
                description = "Test",
                status = 1,
                priority = 3,
                dueDate = DateTime.UtcNow.AddDays(2)
            });

        createTask1Response.StatusCode.Should().Be(HttpStatusCode.OK);

        await _client.PostAsJsonAsync(
            $"/api/Projects/{projectId}/tasks",
            new
            {
                projectId,
                title = "Task Done Low",
                description = "Test",
                status = 3,
                priority = 1,
                dueDate = DateTime.UtcNow.AddDays(2)
            });

        var response = await _client.GetAsync(
            "/api/Tasks?Status=Todo&Priority=High");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());

        var result =
            await response.Content.ReadFromJsonAsync<PagedResult<TaskListDto>>(options);

        result.Should().NotBeNull();
        result!.TotalCount.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items.First().Title.Should().Be("Task Todo High");
    }
}