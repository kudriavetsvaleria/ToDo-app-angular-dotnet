using ToDoApp.Domain.Entities;

namespace ToDoApp.DataAccess.Repositories;

/// <summary>
/// Data-access contract for categories. Mirrors ITaskRepository.
/// </summary>
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(int userId, CancellationToken ct = default);
    Task<Category?> GetByIdAsync(int id, int userId, CancellationToken ct = default);
    Task<Category?> GetByIdWithTasksAsync(int id, int userId, CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
    void Update(Category category);
    void Remove(Category category);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
