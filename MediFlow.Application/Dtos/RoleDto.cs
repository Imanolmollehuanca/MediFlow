using MediFlow.Domain.Entities;
namespace MediFlow.Application.Dtos;

public record RoleDto(int RoleId, string Name, string? Description
)
{
    public static RoleDto FromEntity(Role role)
    {
        return new RoleDto(role.RoleId, role.Name, role.Description
        );
    }
}