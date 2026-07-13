using ToDoApp.DataAccess.Repositories;
using ToDoApp.Domain.Dtos;
using ToDoApp.Domain.Entities;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services;

/// <summary>
/// Implements task business logic. Talks to the repository (data access) and
/// maps between EF entities and API DTOs.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<TaskResponse>> GetTasksAsync(int userId, int page, int pageSize, string? search, int? categoryId)
    {
        // Ask the repository for one page of (optionally filtered) tasks + total count.
        var (tasks, totalCount) = await _repository.GetPagedAsync(userId, page, pageSize, search, categoryId);

        // Round UP: 25 items / 10 per page = 2.5 -> 3 pages (last page is partial).
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Pack everything into the envelope, mapping entities -> DTOs.
        return new PagedResult<TaskResponse>
        {
            Items = tasks.Select(MapToResponse),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<TaskResponse?> GetTaskAsync(int id, int userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        return task is null ? null : MapToResponse(task);
    }

    public async Task<TaskResponse> CreateTaskAsync(int userId, CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            CategoryId = request.CategoryId,
            UserId = userId,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();

        // Re-read so the response includes the category name (if any).
        var created = await _repository.GetByIdAsync(task.Id, userId);
        return MapToResponse(created!);
    }

    public async Task<bool> UpdateTaskAsync(int id, int userId, UpdateTaskRequest request)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task is null)
            return false;

        task.Title = request.Title;
        task.Description = request.Description;
        task.IsCompleted = request.IsCompleted;
        task.DueDate = request.DueDate;
        task.CategoryId = request.CategoryId;

        _repository.Update(task);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task is null)
            return false;

        _repository.Remove(task);
        await _repository.SaveChangesAsync();
        return true;
    }

    private static TaskResponse MapToResponse(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        IsCompleted = task.IsCompleted,
        DueDate = task.DueDate,
        CreatedAt = task.CreatedAt,
        CategoryId = task.CategoryId,
        CategoryName = task.Category?.Name
    };
}
