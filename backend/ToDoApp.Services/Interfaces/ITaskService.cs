using ToDoApp.Domain.Dtos;

namespace ToDoApp.Services.Interfaces;

/// <summary>
/// Business operations for tasks. Controllers depend on this interface.
/// Works with DTOs, never exposing EF entities to the API layer.
/// </summary>
public interface ITaskService
{
    Task<PagedResult<TaskResponse>> GetTasksAsync(int userId, int page, int pageSize, string? search, int? categoryId);
    Task<TaskResponse?> GetTaskAsync(int id, int userId);
    Task<TaskResponse> CreateTaskAsync(int userId, CreateTaskRequest request);
    Task<bool> UpdateTaskAsync(int id, int userId, UpdateTaskRequest request);
    Task<bool> DeleteTaskAsync(int id, int userId);
}
