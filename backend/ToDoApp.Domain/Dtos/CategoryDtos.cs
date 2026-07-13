namespace ToDoApp.Domain.Dtos;

/// <summary>
/// Data the client sends to create OR update a category.
/// Create and update need the same single field (Name), so one DTO covers both.
/// </summary>
public class CategoryRequest
{
    public string Name { get; set; } = string.Empty;
}

/// <summary>Shape of a category as returned by the API.</summary>
public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
