namespace TaskManager.Api.Models;

public class TaskItem
{
    private TaskItem()
    {
    }

    public TaskItem(
        string title,
        TaskPriority priority,
        DateTimeOffset createdAt,
        string? description = null,
        DateTimeOffset? dueDate = null)
    {
        Title = ValidateTitle(title);
        Description = ValidateDescription(description);

        if (!Enum.IsDefined(priority))
        {
            throw new ArgumentOutOfRangeException(nameof(priority));
        }

        Id = Guid.NewGuid();
        Priority = priority;
        Status = TaskItemStatus.Pending;
        DueDate = dueDate?.ToUniversalTime();
        CreatedAt = createdAt.ToUniversalTime();
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public TaskItemStatus Status { get; private set; }

    public TaskPriority Priority { get; private set; }

    public DateTimeOffset? DueDate { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public bool IsArchived { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public bool IsOverdue(DateTimeOffset now) =>
        Status is TaskItemStatus.Pending or TaskItemStatus.InProgress
        && DueDate is not null
        && DueDate < now.ToUniversalTime();

    public void TransitionTo(TaskItemStatus newStatus, DateTimeOffset occurredAt)
    {
        var isAllowed = (Status, newStatus) switch
        {
            (TaskItemStatus.Pending, TaskItemStatus.InProgress or TaskItemStatus.Cancelled) => true,
            (TaskItemStatus.InProgress, TaskItemStatus.Completed or TaskItemStatus.Cancelled) => true,
            _ => false
        };

        if (!isAllowed)
        {
            throw new InvalidOperationException($"Transition from {Status} to {newStatus} is not allowed.");
        }

        Status = newStatus;
        CompletedAt = newStatus == TaskItemStatus.Completed
            ? occurredAt.ToUniversalTime()
            : null;
        UpdatedAt = occurredAt.ToUniversalTime();
    }

    public void Archive(DateTimeOffset occurredAt)
    {
        if (Status != TaskItemStatus.Completed)
        {
            throw new InvalidOperationException("Only completed tasks can be archived.");
        }

        IsArchived = true;
        UpdatedAt = occurredAt.ToUniversalTime();
    }

    public void Unarchive(DateTimeOffset occurredAt)
    {
        if (!IsArchived)
        {
            throw new InvalidOperationException("The task is not archived.");
        }

        IsArchived = false;
        UpdatedAt = occurredAt.ToUniversalTime();
    }

    private static string ValidateTitle(string title)
    {
        var normalizedTitle = title?.Trim();

        if (string.IsNullOrWhiteSpace(normalizedTitle) || normalizedTitle.Length is < 3 or > 100)
        {
            throw new ArgumentException("Title must contain between 3 and 100 non-whitespace characters.", nameof(title));
        }

        return normalizedTitle;
    }

    private static string? ValidateDescription(string? description)
    {
        if (description is { Length: > 2000 })
        {
            throw new ArgumentException("Description cannot exceed 2,000 characters.", nameof(description));
        }

        return description;
    }
}
