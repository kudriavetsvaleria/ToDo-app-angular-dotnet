using ToDoApp.Domain.Entities;

namespace ToDoApp.DataAccess.Repositories;

/// <summary>
/// Data-access contract for tasks. The service layer depends on this interface,
/// not on the concrete EF Core implementation.
/// </summary>
public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(int userId, CancellationToken ct = default);
    Task<TaskItem?> GetByIdAsync(int id, int userId, CancellationToken ct = default);
    Task AddAsync(TaskItem task, CancellationToken ct = default);
    void Update(TaskItem task);
    void Remove(TaskItem task);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
