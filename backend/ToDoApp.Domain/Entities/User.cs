namespace ToDoApp.Domain.Entities;

/// <summary>
/// A registered user. Owns their own tasks and categories.
/// </summary>
public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    /// <summary>Hashed password — we never store the plain password.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    // Navigation properties: one user has many tasks and many categories.
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
