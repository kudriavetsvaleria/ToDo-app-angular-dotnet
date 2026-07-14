using Microsoft.AspNetCore.Mvc;
using ToDoApp.Domain.Dtos;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // -> /api/auth
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        // null => username already taken.
        return result is null
            ? Conflict("Username is already taken.")
            : Ok(result);
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        // null => wrong username or password.
        return result is null
            ? Unauthorized("Invalid username or password.")
            : Ok(result);
    }
}
