namespace ToDoApp.Domain.Dtos;

/// <summary>Data the client sends to register a new account.</summary>
public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>Data the client sends to log in.</summary>
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// What the API returns after a successful register/login:
/// the JWT the client must send on later requests, plus the username.
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
