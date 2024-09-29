using API_CRUDTareas.Controllers;
using API_CRUDTareas.Interfaces;
using API_CRUDTareas.Models.DTOs;
using API_CRUDTareas.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API_CRUDTareas.Tests
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _controller = new TasksController(_taskServiceMock.Object, new AppSettings());
        }

        [Fact]
        public async Task GetTasks_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<TaskDTO>
            {
                new TaskDTO { Id = 1, Title = "Tarea 1", Description = "Descripción 1", IsCompleted = false }
            };
            _taskServiceMock.Setup(service => service.GetTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnTasks = Assert.IsType<List<TaskDTO>>(okResult.Value);
            Assert.Single(returnTasks);
        }

        [Fact]
        public async Task GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            int taskId = 1;
            _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId)).ReturnsAsync((TaskDTO)null);

            // Act
            var result = await _controller.GetTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedAtAction_WhenTaskIsCreated()
        {
            // Arrange
            var createTaskDto = new CreateTaskDTO { Title = "Tarea 1", Description = "Descripción", IsCompleted = false };
            var createdTask = new TaskDTO { Id = 1, Title = "Tarea 1", Description = "Descripción", IsCompleted = false };
            _taskServiceMock.Setup(service => service.CreateTaskAsync(createTaskDto)).ReturnsAsync(createdTask);

            // Act
            var result = await _controller.CreateTask(createTaskDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetTask", createdResult.ActionName);
            Assert.Equal(createdTask.Id, createdResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNoContent_WhenTaskIsUpdated()
        {
            // Arrange
            int taskId = 1;
            var updateTaskDto = new UpdateTaskDTO { Title = "Tarea actualizada", Description = "Descripción actualizada", IsCompleted = true };
            _taskServiceMock.Setup(service => service.UpdateTaskAsync(taskId, updateTaskDto)).ReturnsAsync(new TaskDTO());

            // Act
            var result = await _controller.UpdateTask(taskId, updateTaskDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent_WhenTaskIsDeleted()
        {
            // Arrange
            int taskId = 1;
            _taskServiceMock.Setup(service => service.DeleteTaskAsync(taskId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTask(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            int taskId = 1;
            _taskServiceMock.Setup(service => service.DeleteTaskAsync(taskId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTask(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetTasks_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            _taskServiceMock.Setup(service => service.GetTasksAsync()).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<BadRequestResponse>(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateTask_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.CreateTask(new CreateTaskDTO());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(1, badRequestResult.Value.GetType().GetProperties().Length);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            int taskId = 1;
            var updateTaskDto = new UpdateTaskDTO { Title = "Tarea actualizada", Description = "Descripción actualizada", IsCompleted = true };
            _taskServiceMock.Setup(service => service.UpdateTaskAsync(taskId, updateTaskDto)).ReturnsAsync((TaskDTO)null);

            // Act
            var result = await _controller.UpdateTask(taskId, updateTaskDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
