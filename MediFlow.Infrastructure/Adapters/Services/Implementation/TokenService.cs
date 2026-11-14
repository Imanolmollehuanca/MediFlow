
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediFlow.Application.Port.Services;
using MediFlow.Domain.Entities;
using MediFlow.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediFlow.Infrastructure.Adapters.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public TokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public TokenResult GenerateToken(User user)
    {
        if (string.IsNullOrWhiteSpace(_settings.Key) ||
            Encoding.UTF8.GetByteCount(_settings.Key) < 32)
        {
            throw new InvalidOperationException(
                "La clave JWT debe contener al menos 32 bytes.");
        }

        if (string.IsNullOrWhiteSpace(_settings.Issuer) ||
            string.IsNullOrWhiteSpace(_settings.Audience))
        {
            throw new InvalidOperationException(
                "Jwt:Issuer y Jwt:Audience son obligatorios.");
        }

        if (_settings.ExpirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "Jwt:ExpirationMinutes debe ser mayor que cero.");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email)
        };

        foreach (var roleName in user.UserRoles
                     .Where(ur => ur.Role != null)
                     .Select(ur => ur.Role.Name)
                     .Where(name => !string.IsNullOrWhiteSpace(name))
                     .Distinct())
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(
            _settings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return new TokenResult(accessToken, expiresAt);
    }
}
