using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.Dtos;

/// <summary>Data the client sends to create a task.</summary>
public class CreateTaskRequest
{
    // Lengths mirror the database columns configured in ApplicationDbContext,
    // so an over-long value returns 400 instead of failing at insert time.
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title must be at most 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description must be at most 1000 characters.")]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
    public int? CategoryId { get; set; }
}

/// <summary>Data the client sends to update a task.</summary>
public class UpdateTaskRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title must be at most 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description must be at most 1000 characters.")]
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
