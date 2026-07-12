namespace ToDoApp.Domain.Entities;

/// <summary>
/// A category (e.g. "Work", "Home") used to group tasks. Belongs to one user.
/// </summary>
public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // Foreign key + navigation to the owning user.
    public int UserId { get; set; }
    public User? User { get; set; }

    // One category has many tasks.
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
