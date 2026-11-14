
using MediFlow.Application.Port.Services;
using MediFlow.Domain.Entities;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly MediFlowContext _context;
    private readonly IPasswordService _passwordService;

    public AuthService(
        MediFlowContext context,
        IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    public async Task<User?> ValidateCredentialsAsync(
        string usernameOrEmail,
        string password)
    {
        if (string.IsNullOrWhiteSpace(usernameOrEmail) ||
            string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var user = await _context.Set<User>()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u =>
                u.Username == usernameOrEmail ||
                u.Email == usernameOrEmail);

        if (user == null || !user.IsActive)
            return null;

        var valid = _passwordService.VerifyPassword(
            user.PasswordHash,
            password);

        return valid ? user : null;
    }

    public async Task<bool> ChangePasswordAsync(
        int userId,
        string currentPassword,
        string newPassword)
    {
        if (string.IsNullOrWhiteSpace(currentPassword) ||
            string.IsNullOrWhiteSpace(newPassword))
        {
            return false;
        }

        var user = await _context.Set<User>()
            .FindAsync(userId);

        if (user == null || !user.IsActive)
            return false;

        if (!_passwordService.VerifyPassword(
                user.PasswordHash,
                currentPassword))
        {
            return false;
        }

        user.PasswordHash = _passwordService.HashPassword(
            newPassword);

        await _context.SaveChangesAsync();
        return true;
    }
}
