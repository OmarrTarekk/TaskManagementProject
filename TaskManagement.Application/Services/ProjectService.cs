using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Contracts;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Projects;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Domain.Contracts;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public ProjectService(
       IUnitOfWork unitOfWork,
       IMapper mapper,
       ITaskService taskService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taskService = taskService;
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto,CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<Project>();

            var exists = await repository.AnyAsync(       p => p.Name == dto.Name,ct);

            if (exists)
                throw new Exception("Project name already exists.");

            var project = _mapper.Map<Project>(dto);

            await repository.AddAsync(project, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteProjectAsync(
      int id,
      CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<Project>();

            var project = await repository.GetByIdAsync(id, ct);

            if (project is null)
                return false;

            repository.Delete(project);

            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        public async Task<PagedResult<ProjectListDto>> GetAllProjectsAsync(
            int page,
            int limit,
            CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<Project>();

            var (projects, totalCount) =
    await repository.GetPagedAsync(page, limit, ct);

            return new PagedResult<ProjectListDto>
            {
                Items = _mapper.Map<IReadOnlyList<ProjectListDto>>(projects),
                TotalCount = totalCount,
                Page = page,
                Limit = limit
            };
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(
     int id,
     CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<Project>();

            var project = await repository.GetByIdAsync(id, ct);

            if (project is null)
                return null;

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> UpdateProjectAsync(
       int id,
       UpdateProjectDto dto,
       CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<Project>();

            var project = await repository.GetByIdAsync(id, ct);

            if (project is null)
                return null;

            var exists = await repository.AnyAsync(
                p => p.Name == dto.Name && p.Id != id,
                ct);

            if (exists)
                throw new Exception("Project name already exists.");

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.UpdatedAt = DateTime.UtcNow;

            repository.Update(project);

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<ProjectDto>(project);


        }
        public async Task<TaskDto> CreateTaskAsync(
    int projectId,
    CreateTaskDto dto,
    CancellationToken ct = default)
        {
            dto.ProjectId = projectId;

            return await _taskService.CreateTaskAsync(dto, ct);
        }

        public async Task<IReadOnlyList<TaskListDto>> GetProjectTasksAsync(
    int projectId,
    CancellationToken ct = default)
        {
            return await _taskService.GetTasksByProjectIdAsync(projectId, ct);
        }
    }
}
