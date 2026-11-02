using MediFlow.Application.Dtos;
using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Roles;

public class GetRoleByIdUseCase(IUnitOfWork uow)
{
    public async Task<RoleResponse> ExecuteAsync(int roleId, CancellationToken ct = default)
    {
        var role = await uow.Roles.GetByIdAsync(roleId, ct)
                   ?? throw new NotFoundException($"No se encontró el rol con id {roleId}.");

        return role.ToResponse();
    }
}