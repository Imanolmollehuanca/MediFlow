using MediFlow.Application.Dtos;
using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Roles;

public class UpdateRoleUseCase(IUnitOfWork uow)
{
    public async Task<RoleResponse> ExecuteAsync(int roleId, UpdateRoleRequest request, CancellationToken ct = default)
    {
        var role = await uow.Roles.GetByIdAsync(roleId, ct)
                   ?? throw new NotFoundException($"No se encontró el rol con id {roleId}.");

        var name = request.Name.Trim();

        if (await uow.Roles.ExistsByNameAsync(name, roleId, ct))
            throw new ConflictException($"Ya existe un rol con el nombre '{name}'.");

        role.Name = name;
        role.Description = request.Description?.Trim();

        await uow.SaveChangesAsync(ct);

        return role.ToResponse();
    }
}