using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Roles.Commands;

public record UpdateRoleCommand(int RoleId, UpdateRoleDto Dto)
    : IRequest<RoleDto?>;

public class UpdateRoleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRoleCommand, RoleDto?>
{
    public async Task<RoleDto?> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(request.RoleId);

        if (role is null)
            return null;

        var dto = request.Dto;
        var name = dto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del rol es obligatorio.");

        var existingRole = await unitOfWork.Roles.GetByNameAsync(name);

        if (existingRole is not null &&
            existingRole.RoleId != request.RoleId)
        {
            throw new InvalidOperationException(
                "Ya existe otro rol con ese nombre.");
        }

        role.Name = name;
        role.Description = dto.Description?.Trim();

        await unitOfWork.Roles.UpdateAsync(role);
        await unitOfWork.SaveChangesAsync();

        return RoleDto.FromEntity(role);
    }
}
