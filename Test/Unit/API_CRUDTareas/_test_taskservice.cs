using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Models.Context;
using API_CRUDTareas.Models.DTOs;
using API_CRUDTareas.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace API_CRUDTareas.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly IMapper _mapper;
        private readonly TasksContext _context;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            // Configuración de AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TaskModel, TaskDTO>();
                cfg.CreateMap<CreateTaskDTO, TaskModel>();
                cfg.CreateMap<UpdateTaskDTO, TaskModel>();
            });
            _mapper = config.CreateMapper();

            // Configuración de la base de datos en memoria
            var options = new DbContextOptionsBuilder<TasksContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TasksContext(options);
            _jwtServiceMock = new Mock<IJwtService>();
            _taskService = new TaskService(_context, _mapper, _jwtServiceMock.Object);
        }

        [Fact]
        public async Task GetTasksAsync_ReturnsListOfTasks()
        {
            // Arrange
            var taskModel = new TaskModel { Id = 1, Title = "Test Task", Description = "Test Description" };
            _context.Tasks.Add(taskModel);
            await _context.SaveChangesAsync();

            // Act
            var tasks = await _taskService.GetTasksAsync();

            // Assert
            Assert.Single(tasks);
            Assert.Equal("Test Task", tasks.First().Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask_WhenExists()
        {
            // Arrange
            var taskModel = new TaskModel { Id = 1, Title = "Test Task" };
            _context.Tasks.Add(taskModel);
            await _context.SaveChangesAsync();

            // Act
            var task = await _taskService.GetTaskByIdAsync(1);

            // Assert
            Assert.NotNull(task);
            Assert.Equal("Test Task", task.Title);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Act
            var task = await _taskService.GetTaskByIdAsync(999);

            // Assert
            Assert.Null(task);
        }

        [Fact]
        public async Task CreateTaskAsync_CreatesTask()
        {
            // Arrange
            var createTaskDto = new CreateTaskDTO { Title = "New Task", Description = "New Description" };
            _jwtServiceMock.Setup(x => x.GetUsernameFromContext()).Returns("testuser");

            // Act
            var createdTask = await _taskService.CreateTaskAsync(createTaskDto);

            // Assert
            Assert.NotNull(createdTask);
            Assert.Equal("New Task", createdTask.Title);
            Assert.Equal("testuser", createdTask.UserCreated);
        }

        [Fact]
        public async Task UpdateTaskAsync_UpdatesTask_WhenExists()
        {
            // Arrange
            var taskModel = new TaskModel { Id = 1, Title = "Old Task" };
            _context.Tasks.Add(taskModel);
            await _context.SaveChangesAsync();

            var updateTaskDto = new UpdateTaskDTO { Title = "Updated Task", Description = "Updated Description" };
            _jwtServiceMock.Setup(x => x.GetUsernameFromContext()).Returns("testuser");

            // Act
            var updatedTask = await _taskService.UpdateTaskAsync(1, updateTaskDto);

            // Assert
            Assert.NotNull(updatedTask);
            Assert.Equal("Updated Task", updatedTask.Title);
            Assert.Equal("testuser", updatedTask.UserModified);
        }

        [Fact]
        public async Task UpdateTaskAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var updateTaskDto = new UpdateTaskDTO { Title = "Updated Task" };

            // Act
            var updatedTask = await _taskService.UpdateTaskAsync(999, updateTaskDto);

            // Assert
            Assert.Null(updatedTask);
        }

        [Fact]
        public async Task DeleteTaskAsync_ReturnsTrue_WhenDeleted()
        {
            // Arrange
            var taskModel = new TaskModel { Id = 1, Title = "Task to Delete" };
            _context.Tasks.Add(taskModel);
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.DeleteTaskAsync(1);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Tasks);
        }

        [Fact]
        public async Task DeleteTaskAsync_ReturnsFalse_WhenNotExists()
        {
            // Act
            var result = await _taskService.DeleteTaskAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}
