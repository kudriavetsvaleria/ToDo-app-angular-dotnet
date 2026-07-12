using Microsoft.EntityFrameworkCore;
using ToDoApp.DataAccess.Persistence;
using ToDoApp.Domain.Entities;

namespace ToDoApp.DataAccess.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ITaskRepository"/>.
/// Every query is filtered by userId so users only ever see their own tasks.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _db;

    public TaskRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<TaskItem>> GetAllAsync(int userId, CancellationToken ct = default)
    {
        return await _db.Tasks
            .Include(t => t.Category)   // load the related category so we can show its name
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<TaskItem?> GetByIdAsync(int id, int userId, CancellationToken ct = default)
    {
        return await _db.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, ct);
    }

    public async Task AddAsync(TaskItem task, CancellationToken ct = default)
    {
        await _db.Tasks.AddAsync(task, ct);
    }

    public void Update(TaskItem task) => _db.Tasks.Update(task);

    public void Remove(TaskItem task) => _db.Tasks.Remove(task);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
