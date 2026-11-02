using System.ComponentModel.DataAnnotations;
using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record CreateUserRequest(
    [Required, StringLength(50, MinimumLength = 3)] string Username,
    [Required, EmailAddress, StringLength(150)] string Email,
    [Required, StringLength(100, MinimumLength = 8)] string Password,
    IEnumerable<int>? RoleIds);

public record UpdateUserRequest(
    [Required, StringLength(50, MinimumLength = 3)] string Username,
    [Required, EmailAddress, StringLength(150)] string Email,
    bool IsActive,
    [StringLength(100, MinimumLength = 8)] string? NewPassword);

public record UserResponse(
    int UserId,
    string Username,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyList<RoleResponse> Roles);

public static class UserMappings
{
    // Nunca se  expondra  nuestro  PasswordHash
    public static UserResponse ToResponse(this User user) =>
        new(user.UserId,
            user.Username,
            user.Email,
            user.IsActive,
            user.CreatedAt,
            user.UserRoles
                .Where(ur => ur.Role is not null)
                .Select(ur => ur.Role.ToResponse())
                .ToList());
}