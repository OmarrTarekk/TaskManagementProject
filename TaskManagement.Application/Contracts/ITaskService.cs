using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Contracts
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(
            CreateTaskDto dto,
            CancellationToken ct = default);

        Task<PagedResult<TaskListDto>> GetAllTasksAsync(
    TaskQueryParameters parameters,
    CancellationToken ct = default);

        Task<TaskDto?> GetTaskByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<TaskDto?> UpdateTaskAsync(
            int id,
            UpdateTaskDto dto,
            CancellationToken ct = default);

        Task<bool> DeleteTaskAsync(
            int id,
            CancellationToken ct = default);

        Task<IReadOnlyList<TaskListDto>> GetTasksByProjectIdAsync(
    int projectId,
    CancellationToken ct = default);
    }
}