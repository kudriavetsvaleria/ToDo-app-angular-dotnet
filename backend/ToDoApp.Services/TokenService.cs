using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Domain.Entities;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public TokenService(JwtSettings settings)
    {
        _settings = settings;
    }

    public string GenerateToken(User user)
    {
        // 1. Claims — the facts we write inside the token (payload).
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // who: the user id
            new Claim(ClaimTypes.Name, user.Username)                 // and the username
        };

        // 2. The signing key (our secret) + algorithm.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 3. Assemble the token: issuer, audience, claims, expiry, signature.
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: credentials);

        // 4. Serialize to the compact "header.payload.signature" string.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
