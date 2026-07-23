using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using TaskManagement.Application.Contracts;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Contracts;
using TaskManagement.Domain.Entities;
using Xunit;

namespace TaskManagement.Tests.Unit
{
    public class TaskServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepository<Project>> _projectRepositoryMock;
        private readonly Mock<IGenericRepository<TaskItem>> _taskRepositoryMock;
        private readonly IMapper _mapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _projectRepositoryMock = new Mock<IGenericRepository<Project>>();
            _taskRepositoryMock = new Mock<IGenericRepository<TaskItem>>();

            _unitOfWorkMock
                .Setup(x => x.Repository<Project>())
                .Returns(_projectRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Repository<TaskItem>())
                .Returns(_taskRepositoryMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();

            _taskService = new TaskService(
                _unitOfWorkMock.Object,
                _mapper);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldThrowException_WhenDueDateIsInPast()
        {
         
            var project = new Project
            {
                Id = 1,
                Name = "Backend"
            };

            _projectRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(project);

            var dto = new CreateTaskDto
            {
                ProjectId = 1,
                Title = "Login",
                Priority = Domain.Entities.Enums.TaskPriority.High,
                Status = Domain.Entities.Enums.TaskItemStatus.Todo,
                DueDate = DateTime.UtcNow.AddDays(-1)
            };

        
            Func<Task> action = async () =>
                await _taskService.CreateTaskAsync(dto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Due Date cannot be in the past.");
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldThrowException_WhenProjectDoesNotExist()
        {
            _projectRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project?)null);

            var dto = new CreateTaskDto
            {
                ProjectId = 1,
                Title = "Login",
                Priority = Domain.Entities.Enums.TaskPriority.High,
                Status = Domain.Entities.Enums.TaskItemStatus.Todo,
                DueDate = DateTime.UtcNow.AddDays(2)
            };

            Func<Task> action = async () =>
                await _taskService.CreateTaskAsync(dto);

            
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Project not found.");
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldCreateTaskSuccessfully()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                Name = "Backend"
            };

            _projectRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(project);

            _taskRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var dto = new CreateTaskDto
            {
                ProjectId = 1,
                Title = "Login",
                Description = "JWT",
                Status = Domain.Entities.Enums.TaskItemStatus.Todo,
                Priority = Domain.Entities.Enums.TaskPriority.High,
                DueDate = DateTime.UtcNow.AddDays(2)
            };

            var result = await _taskService.CreateTaskAsync(dto);

          
            result.Should().NotBeNull();
            result.Title.Should().Be("Login");

            _taskRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTaskSuccessfully()
        {
            var task = new TaskItem
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old Description",
                Status = Domain.Entities.Enums.TaskItemStatus.Todo,
                Priority = Domain.Entities.Enums.TaskPriority.Low
            };

            _taskRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var dto = new UpdateTaskDto
            {
                Title = "New Title",
                Description = "New Description",
                Status = Domain.Entities.Enums.TaskItemStatus.Done,
                Priority = Domain.Entities.Enums.TaskPriority.High
            };

            var result = await _taskService.UpdateTaskAsync(1, dto);

            result.Should().NotBeNull();
            result!.Title.Should().Be("New Title");

            _taskRepositoryMock.Verify(x => x.Update(It.IsAny<TaskItem>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTaskSuccessfully()
        {
            var task = new TaskItem
            {
                Id = 1,
                Title = "Login"
            };

            _taskRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _taskService.DeleteTaskAsync(1);

            result.Should().BeTrue();

            _taskRepositoryMock.Verify(x => x.Delete(It.IsAny<TaskItem>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}