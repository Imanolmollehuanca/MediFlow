using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Roles;

public class CreateRoleUseCase(IUnitOfWork uow)
{
    public async Task<RoleResponse> ExecuteAsync(CreateRoleRequest request, CancellationToken ct = default)
    {
        var name = request.Name.Trim();

        if (await uow.Roles.ExistsByNameAsync(name, null, ct))
            throw new ConflictException($"Ya existe un rol con el nombre '{name}'.");

        var role = new Role { Name = name, Description = request.Description?.Trim() };

        await uow.Roles.AddAsync(role, ct);
        await uow.SaveChangesAsync(ct);

        return role.ToResponse();
    }
}