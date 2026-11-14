using MediFlow.Domain.Entities;

namespace MediFlow.Application.Port.Services;

public interface IAuthService
{
    Task<User?> ValidateCredentialsAsync(
        string usernameOrEmail,
        string password);

    Task<bool> ChangePasswordAsync(
        int userId,
        string currentPassword,
        string newPassword);
}