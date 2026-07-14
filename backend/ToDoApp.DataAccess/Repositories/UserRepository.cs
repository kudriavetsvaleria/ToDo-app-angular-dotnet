using Microsoft.EntityFrameworkCore;
using ToDoApp.DataAccess.Persistence;
using ToDoApp.Domain.Entities;

namespace ToDoApp.DataAccess.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IUserRepository"/>.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    // Find a user by username. Used at login.
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
    }

    // Quick check used at registration to reject duplicate usernames.
    public async Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default)
    {
        return await _db.Users.AnyAsync(u => u.Username == username, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _db.Users.AddAsync(user, ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
