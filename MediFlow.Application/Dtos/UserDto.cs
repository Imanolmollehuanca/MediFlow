using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record UserDto(
    int UserId, string Username, string Email, bool IsActive,
    DateTime CreatedAt, List<string> Roles
)
{
    public static UserDto FromEntity(User user)
    {
        return new UserDto(user.UserId, user.Username, user.Email, user.IsActive, user.CreatedAt, user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToList()
        );
    }
}