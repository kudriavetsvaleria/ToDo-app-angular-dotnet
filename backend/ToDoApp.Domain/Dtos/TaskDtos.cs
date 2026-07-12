namespace ToDoApp.Domain.Dtos;

/// <summary>Data the client sends to create a task.</summary>
public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int? CategoryId { get; set; }
}

/// <summary>Data the client sends to update a task.</summary>
public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public int? CategoryId { get; set; }
}

/// <summary>Shape of a task as returned by the API.</summary>
public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
