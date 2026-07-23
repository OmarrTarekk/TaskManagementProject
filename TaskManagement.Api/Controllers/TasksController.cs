using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTaskDto dto,
            CancellationToken ct)
        {
            var result = await _taskService.CreateTaskAsync(dto, ct);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] TaskQueryParameters parameters,
    CancellationToken ct)
        {
            var result = await _taskService.GetAllTasksAsync(parameters, ct);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            var result = await _taskService.GetTaskByIdAsync(id, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateTaskDto dto,
            CancellationToken ct)
        {
            var result = await _taskService.UpdateTaskAsync(id, dto, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            var deleted = await _taskService.DeleteTaskAsync(id, ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}