using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.Api;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new TaskDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<TaskDbContext>>());
        if (context.Tasks.Any()) return;
        context.Tasks.AddRange(
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Sample Task 1",
                Description = "This is a sample task.",
                Status = Models.TaskStatus.Todo,
                Priority = TaskPriority.Medium,
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7)
            },
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Sample Task 2",
                Description = "Another sample task.",
                Status = Models.TaskStatus.InProgress,
                Priority = TaskPriority.High,
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3)
            }
        );
        context.SaveChanges();
    }
}
