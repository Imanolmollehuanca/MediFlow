using System.Security.Claims;
using MediFlow.Application.Port.Services;
using Microsoft.AspNetCore.Http;

namespace MediFlow.Infrastructure.Adapters.Services.Implementation;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public int? UserId
    {
        get
        {
            var value = _accessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(value, out var id)
                ? id
                : null;
        }
    }

    public string? Username =>
        _accessor.HttpContext?.User.Identity?.Name;

    public bool IsAuthenticated =>
        _accessor.HttpContext?.User.Identity?.IsAuthenticated
        ?? false;
}