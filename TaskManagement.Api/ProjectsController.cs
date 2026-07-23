using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateProjectDto dto,
            CancellationToken ct)
        {
            var result = await _projectService.CreateProjectAsync(dto, ct);

            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
     [FromQuery] int page = 1,
     [FromQuery] int limit = 10,
     CancellationToken ct = default)
        {
            var result = await _projectService.GetAllProjectsAsync(page, limit, ct);

            return Ok(result);
        }


        [HttpPost("{id}/tasks")]
        public async Task<IActionResult> CreateTask(
            int id,
            [FromBody] CreateTaskDto dto,
            CancellationToken ct)
        {
            var result = await _projectService.CreateTaskAsync(id, dto, ct);

            return Ok(result);
        }

        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> GetProjectTasks(
            int id,
            CancellationToken ct)
        {
            var result = await _projectService.GetProjectTasksAsync(id, ct);

            return Ok(result);
        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            var result = await _projectService.GetProjectByIdAsync(id, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateProjectDto dto,
            CancellationToken ct)
        {
            var result = await _projectService.UpdateProjectAsync(id, dto, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            var deleted = await _projectService.DeleteProjectAsync(id, ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}