using TaskManager.Api.Models;

namespace TaskManager.UnitTests;

public class TaskItemTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 22, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void NewTaskStartsPendingAndTrimsTitle()
    {
        var task = new TaskItem("  Review PR  ", TaskPriority.High, Now);

        Assert.Equal("Review PR", task.Title);
        Assert.Equal(TaskItemStatus.Pending, task.Status);
        Assert.Null(task.CompletedAt);
        Assert.False(task.IsArchived);
    }

    [Theory]
    [InlineData("  ")]
    [InlineData("ab")]
    public void ConstructorRejectsInvalidTitle(string title)
    {
        Assert.Throws<ArgumentException>(() => new TaskItem(title, TaskPriority.Medium, Now));
    }

    [Fact]
    public void TaskCanCompleteOnlyAfterBeingStartedAndRecordsCompletionTime()
    {
        var task = new TaskItem("Implement tests", TaskPriority.Medium, Now);

        task.TransitionTo(TaskItemStatus.InProgress, Now.AddMinutes(1));
        task.TransitionTo(TaskItemStatus.Completed, Now.AddMinutes(2));

        Assert.Equal(TaskItemStatus.Completed, task.Status);
        Assert.Equal(Now.AddMinutes(2), task.CompletedAt);
    }

    [Fact]
    public void PendingTaskCannotBeCompletedDirectly()
    {
        var task = new TaskItem("Implement tests", TaskPriority.Medium, Now);

        Assert.Throws<InvalidOperationException>(() => task.TransitionTo(TaskItemStatus.Completed, Now));
    }

    [Fact]
    public void OnlyCompletedTaskCanBeArchivedAndThenUnarchived()
    {
        var task = new TaskItem("Implement tests", TaskPriority.Medium, Now);

        Assert.Throws<InvalidOperationException>(() => task.Archive(Now));

        task.TransitionTo(TaskItemStatus.InProgress, Now.AddMinutes(1));
        task.TransitionTo(TaskItemStatus.Completed, Now.AddMinutes(2));
        task.Archive(Now.AddMinutes(3));
        task.Unarchive(Now.AddMinutes(4));

        Assert.False(task.IsArchived);
        Assert.Equal(TaskItemStatus.Completed, task.Status);
    }

    [Fact]
    public void OverdueIsCalculatedOnlyForNonTerminalTasks()
    {
        var task = new TaskItem("Review release", TaskPriority.Low, Now, dueDate: Now.AddMinutes(-1));

        Assert.True(task.IsOverdue(Now));

        task.TransitionTo(TaskItemStatus.Cancelled, Now);

        Assert.False(task.IsOverdue(Now));
    }
}
