using MediFlow.Application.Port.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUser _currentUser;

    public AuthController(
        IAuthService authService,
        ITokenService tokenService,
        ICurrentUser currentUser)
    {
        _authService = authService;
        _tokenService = tokenService;
        _currentUser = currentUser;
    }

    public sealed record LoginRequest(
        string UsernameOrEmail,
        string Password);

    public sealed record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var user = await _authService.ValidateCredentialsAsync(
            request.UsernameOrEmail,
            request.Password);

        if (user is null)
            return Unauthorized(new
            {
                message = "Credenciales incorrectas."
            });

        var token = _tokenService.GenerateToken(user);

        return Ok(new
        {
            token.AccessToken,
            token.ExpiresAt,
            user.UserId,
            user.Username,
            user.Email
        });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserId = _currentUser.UserId,
            Username = _currentUser.Username,
            IsAuthenticated = _currentUser.IsAuthenticated
        });
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request)
    {
        if (_currentUser.UserId is not int userId)
            return Unauthorized();

        var changed = await _authService.ChangePasswordAsync(
            userId,
            request.CurrentPassword,
            request.NewPassword);

        if (!changed)
            return BadRequest(new
            {
                message = "No se pudo cambiar la contraseña."
            });

        return Ok(new
        {
            message = "Contraseña actualizada correctamente."
        });
    }
}
