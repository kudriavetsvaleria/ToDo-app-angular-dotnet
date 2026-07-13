using Microsoft.EntityFrameworkCore;
using ToDoApp.DataAccess.Persistence;
using ToDoApp.Domain.Entities;

namespace ToDoApp.DataAccess.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ICategoryRepository"/>.
/// Every query is filtered by userId so users only see their own categories.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAllAsync(int userId, CancellationToken ct = default)
    {
        return await _db.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public async Task<Category?> GetByIdAsync(int id, int userId, CancellationToken ct = default)
    {
        return await _db.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, ct);
    }

    // Loads the category together with its tasks (needed before deleting, so we
    // can detach the tasks first).
    public async Task<Category?> GetByIdWithTasksAsync(int id, int userId, CancellationToken ct = default)
    {
        return await _db.Categories
            .Include(c => c.Tasks)
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, ct);
    }

    public async Task AddAsync(Category category, CancellationToken ct = default)
    {
        await _db.Categories.AddAsync(category, ct);
    }

    public void Update(Category category) => _db.Categories.Update(category);

    public void Remove(Category category) => _db.Categories.Remove(category);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
