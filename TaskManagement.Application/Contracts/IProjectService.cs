using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Contracts
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto,
            CancellationToken ct = default);

        Task<PagedResult<ProjectListDto>> GetAllProjectsAsync(
     int page,
     int limit,
     CancellationToken ct = default);

        Task<ProjectDto?> GetProjectByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<ProjectDto?> UpdateProjectAsync(
            int id,
            UpdateProjectDto dto,
            CancellationToken ct = default);

        Task<bool> DeleteProjectAsync(
            int id,
            CancellationToken ct = default);

        Task<TaskDto> CreateTaskAsync(
    int projectId,
    CreateTaskDto dto,
    CancellationToken ct = default);

        Task<IReadOnlyList<TaskListDto>> GetProjectTasksAsync(
            int projectId,
            CancellationToken ct = default);
    }
}
