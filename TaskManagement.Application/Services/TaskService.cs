using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Contracts;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Domain.Contracts;
using TaskManagement.Domain.Entities;


namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto,CancellationToken ct = default)
    {
        var projectRepository = _unitOfWork.Repository<Project>();

        var project = await projectRepository.GetByIdAsync(dto.ProjectId, ct);

        if (project is null)
            throw new Exception("Project not found.");

        if (dto.DueDate.HasValue && dto.DueDate.Value < DateTime.UtcNow)
            throw new Exception("Due Date cannot be in the past.");

        var taskRepository = _unitOfWork.Repository<TaskItem>();

        var task = _mapper.Map<TaskItem>(dto);

        await taskRepository.AddAsync(task, ct);

        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<TaskDto>(task);
    }

        public async Task<PagedResult<TaskListDto>> GetAllTasksAsync(
      TaskQueryParameters parameters,
      CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<TaskItem>();

            var tasks = await repository.GetAllIncludingAsync( ct,t => t.Project);
            if (parameters.Status.HasValue)
            {
                tasks = tasks
                    .Where(t => t.Status == parameters.Status.Value)
                    .ToList();
            }

            if (parameters.Priority.HasValue)
            {
                tasks = tasks
                    .Where(t => t.Priority == parameters.Priority.Value)
                    .ToList();
            }

            if (parameters.DueDateFrom.HasValue)
            {
                tasks = tasks
                    .Where(t => t.DueDate >= parameters.DueDateFrom.Value)
                    .ToList();
            }

            if (parameters.DueDateTo.HasValue)
            {
                tasks = tasks
                    .Where(t => t.DueDate <= parameters.DueDateTo.Value)
                    .ToList();
            }


            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                switch (parameters.SortBy.ToLower())
                {
                    case "duedate":
                        tasks = parameters.SortOrder?.ToLower() == "desc"
                            ? tasks.OrderByDescending(t => t.DueDate).ToList()
                            : tasks.OrderBy(t => t.DueDate).ToList();
                        break;

                    case "priority":
                        tasks = parameters.SortOrder?.ToLower() == "desc"
                            ? tasks.OrderByDescending(t => t.Priority).ToList()
                            : tasks.OrderBy(t => t.Priority).ToList();
                        break;

                    case "createdat":
                        tasks = parameters.SortOrder?.ToLower() == "desc"
                            ? tasks.OrderByDescending(t => t.CreatedAt).ToList()
                            : tasks.OrderBy(t => t.CreatedAt).ToList();
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                tasks = tasks
                    .Where(t =>
                        t.Title.Contains(parameters.Q, StringComparison.OrdinalIgnoreCase) ||
                        (t.Description != null &&
                         t.Description.Contains(parameters.Q, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
            var totalCount = tasks.Count;

            tasks = tasks
                .Skip((parameters.Page - 1) * parameters.Limit)
                .Take(parameters.Limit)
                .ToList();

            return new PagedResult<TaskListDto>
            {
                Items = _mapper.Map<IReadOnlyList<TaskListDto>>(tasks),
                TotalCount = totalCount,
                Page = parameters.Page,
                Limit = parameters.Limit
            };
        }


        public async Task<TaskDto?> GetTaskByIdAsync(
     int id,
     CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<TaskItem>();

            var task = await repository.GetByIdAsync(id, ct);

            if (task is null)
                return null;

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto?> UpdateTaskAsync(
      int id,
      UpdateTaskDto dto,
      CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<TaskItem>();

            var task = await repository.GetByIdAsync(id, ct);

            if (task is null)
                return null;

            _mapper.Map(dto, task);

            repository.Update(task);

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<bool> DeleteTaskAsync(
     int id,
     CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<TaskItem>();

            var task = await repository.GetByIdAsync(id, ct);

            if (task is null)
                return false;

            repository.Delete(task);

            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        public async Task<IReadOnlyList<TaskListDto>> GetTasksByProjectIdAsync(
    int projectId,
    CancellationToken ct = default)
        {
            var repository = _unitOfWork.Repository<TaskItem>();

            var tasks = await repository.GetAllAsync(ct);

            tasks = tasks
                .Where(t => t.ProjectId == projectId)
                .ToList();

            return _mapper.Map<IReadOnlyList<TaskListDto>>(tasks);
        }

    }
}