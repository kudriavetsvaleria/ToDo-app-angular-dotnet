using ToDoApp.Domain.Entities;

namespace ToDoApp.Services.Interfaces;

/// <summary>Creates a signed JWT for an authenticated user.</summary>
public interface ITokenService
{
    string GenerateToken(User user);
}
