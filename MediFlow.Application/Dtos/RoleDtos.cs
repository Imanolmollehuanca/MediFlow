using System.ComponentModel.DataAnnotations;
using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record CreateRoleRequest(
    [Required, StringLength(50, MinimumLength = 2)] string Name,
    [StringLength(250)] string? Description);

public record UpdateRoleRequest(
    [Required, StringLength(50, MinimumLength = 2)] string Name,
    [StringLength(250)] string? Description);

public record RoleResponse(int RoleId, string Name, string? Description);

public static class RoleMappings
{
    public static RoleResponse ToResponse(this Role role) =>
        new(role.RoleId, role.Name, role.Description);
}