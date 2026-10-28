using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs;

public class TaskResponseDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public TaskItemStatus Status { get; init; }

    public TaskPriority Priority { get; init; }

    public DateTimeOffset? DueDate { get; init; }

    public DateTimeOffset? CompletedAt { get; init; }

    public bool IsArchived { get; init; }

    public bool IsOverdue { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset UpdatedAt { get; init; }

    public static TaskResponseDto FromEntity(TaskItem task, DateTimeOffset now) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        Priority = task.Priority,
        DueDate = task.DueDate,
        CompletedAt = task.CompletedAt,
        IsArchived = task.IsArchived,
        IsOverdue = task.IsOverdue(now),
        CreatedAt = task.CreatedAt,
        UpdatedAt = task.UpdatedAt
    };
}
