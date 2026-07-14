 using ToDoApp.DataAccess.Repositories;
using ToDoApp.Domain.Dtos;
using ToDoApp.Domain.Entities;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository users, ITokenService tokenService)
    {
        _users = users;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        // 1. Reject if the username is already taken.
        if (await _users.UsernameExistsAsync(request.Username))
            return null;

        // 2. Create the user, storing only the HASH of the password.
        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };
        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        // 3. Issue a token so the user is logged in right after registering.
        return BuildResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        // 1. Find the user by username.
        var user = await _users.GetByUsernameAsync(request.Username);
        if (user is null)
            return null;

        // 2. Check the password against the stored hash.
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        // 3. Credentials are good — issue a token.
        return BuildResponse(user);
    }

    private AuthResponse BuildResponse(User user) => new()
    {
        Token = _tokenService.GenerateToken(user),
        Username = user.Username
    };
}
