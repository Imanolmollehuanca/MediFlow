using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Roles;

public class DeleteRoleUseCase(IUnitOfWork uow)
{
    public async Task ExecuteAsync(int roleId, CancellationToken ct = default)
    {
        var role = await uow.Roles.GetByIdAsync(roleId, ct)
                   ?? throw new NotFoundException($"No se encontró el rol con id {roleId}.");

        if (await uow.Roles.HasUsersAsync(roleId, ct))
            throw new ConflictException("No se puede eliminar el rol porque tiene usuarios asignados.");

        uow.Roles.Remove(role);
        await uow.SaveChangesAsync(ct);
    }
}