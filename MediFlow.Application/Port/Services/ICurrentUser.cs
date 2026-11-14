namespace MediFlow.Application.Port.Services;

public interface ICurrentUser
{
    int? UserId { get; }

    string? Username { get; }

    bool IsAuthenticated { get; }
}