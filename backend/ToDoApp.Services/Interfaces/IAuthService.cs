using ToDoApp.Domain.Dtos;

namespace ToDoApp.Services.Interfaces;

/// <summary>
/// Registration and login. Returns an AuthResponse (with a JWT) on success,
/// or null on failure (username taken / wrong credentials).
/// </summary>
public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}
