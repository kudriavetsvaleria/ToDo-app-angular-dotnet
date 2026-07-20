using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Domain.Dtos;

/// <summary>Data the client sends to register a new account.</summary>
public class RegisterRequest
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
    [MaxLength(30, ErrorMessage = "Username must be at most 30 characters.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>Data the client sends to log in.</summary>
public class LoginRequest
{
    // Only "must be present" here: length rules belong to registration.
    // An existing account may have a password shorter than today's rules.
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
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
