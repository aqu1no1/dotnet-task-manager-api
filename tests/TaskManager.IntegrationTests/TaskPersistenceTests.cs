using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.IntegrationTests;

public class TaskPersistenceTests
{
    [Fact]
    public async Task TaskCanBePersistedAndReadBackFromPostgreSql()
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Set ConnectionStrings__DefaultConnection to a dedicated test database.");
        }

        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new TaskManagerDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var task = new TaskItem("Persist integration test", TaskPriority.High, DateTimeOffset.UtcNow);
        context.TaskItems.Add(task);
        await context.SaveChangesAsync();

        var persistedTask = await context.TaskItems
            .AsNoTracking()
            .SingleAsync(item => item.Id == task.Id);

        Assert.Equal(task.Title, persistedTask.Title);
        Assert.Equal(TaskItemStatus.Pending, persistedTask.Status);
        Assert.Equal(TaskPriority.High, persistedTask.Priority);
    }
}
