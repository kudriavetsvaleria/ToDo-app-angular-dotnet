namespace ToDoApp.Domain.Entities;

/// <summary>
/// A single to-do item. Named TaskItem (not Task) to avoid clashing with
/// System.Threading.Tasks.Task.
/// </summary>
public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    /// <summary>Optional deadline.</summary>
    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    // Optional category: a task may be uncategorized (nullable FK).
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    // Required owner: every task belongs to a user.
    public int UserId { get; set; }
    public User? User { get; set; }
}
