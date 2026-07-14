using ToDoApp.Domain.Entities;

namespace ToDoApp.DataAccess.Repositories;

/// <summary>
/// Data-access contract for users (used by registration and login).
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
