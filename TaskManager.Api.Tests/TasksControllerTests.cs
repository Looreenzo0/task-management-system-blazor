using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Controllers;
using TaskManager.Api.Data;
using TaskManager.Api.Models;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace TaskManager.Api.Tests
{
    public class TasksControllerTests
    {
        private TaskDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            return new TaskDbContext(options);
        }

        [Fact]
        public async Task GetTasks_ReturnsOk_WithPagination()
        {
            // Arrange
            var context = GetDbContext();
            context.Tasks.Add(new TaskItem { Title = "Test", Status = TaskStatus.Todo, Priority = TaskPriority.Medium, CreatedDate = System.DateTime.UtcNow });
            context.SaveChanges();
            var controller = new TasksController(context);

            // Act
            var result = await controller.GetTasks(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tasks = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<TaskItem>>(okResult.Value);
            Assert.Single(tasks);
        }

        [Fact]
        public async Task CreateTask_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new TasksController(context);
            controller.ModelState.AddModelError("Title", "Required");
            var task = new TaskItem();

            // Act
            var result = await controller.CreateTask(task);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateTask_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new TasksController(context);
            var task = new TaskItem { Id = System.Guid.NewGuid(), Title = "Test", Status = TaskStatus.Todo, Priority = TaskPriority.Medium, CreatedDate = System.DateTime.UtcNow };

            // Act
            var result = await controller.UpdateTask(System.Guid.NewGuid(), task);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID in URL does not match ID in body.", badRequest.Value);
        }
    }
}
