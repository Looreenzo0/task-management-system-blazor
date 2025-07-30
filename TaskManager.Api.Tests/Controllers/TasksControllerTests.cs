[Fact]
public async Task GetTasks_ReturnsPaginatedTasks()
{
    // Arrange
    var options = new DbContextOptionsBuilder<TaskDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;
    using var context = new TaskDbContext(options);
    context.Tasks.Add(new TaskItem { Id = Guid.NewGuid(), Title = "Test", Status = TaskStatus.Todo, Priority = TaskPriority.Medium, CreatedDate = DateTime.UtcNow });
    context.SaveChanges();

    var controller = new TasksController(context);

    // Act
    var result = await controller.GetTasks();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var tasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);
    Assert.Single(tasks);
}