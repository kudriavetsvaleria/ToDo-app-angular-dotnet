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

    public async Task<(List<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int userId, int page, int pageSize, string? search, int? categoryId, CancellationToken ct = default)
    {
        // Start with "this user's tasks" (not executed yet — just a description).
        var query = _db.Tasks
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        // Sieve 1: filter by category, only if one was provided.
        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        // Sieve 2: search by title, only if search text was provided.
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Title.Contains(search));

        // Count what's left AFTER the sieves (so paging is correct).
        var totalCount = await query.CountAsync(ct);

        // Then order and take the requested page.
        var items = await query
            .OrderBy(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
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
