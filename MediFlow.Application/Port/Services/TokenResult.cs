namespace MediFlow.Application.Port.Services;

public sealed record TokenResult(
    string AccessToken,
    DateTime ExpiresAt
);