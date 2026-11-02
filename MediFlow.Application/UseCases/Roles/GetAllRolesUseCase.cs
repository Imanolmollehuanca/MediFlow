using MediFlow.Application.Dtos;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Roles;

public class GetAllRolesUseCase(IUnitOfWork uow)
{
    public async Task<IReadOnlyList<RoleResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var roles = await uow.Roles.GetAllAsync(ct);
        return roles.Select(r => r.ToResponse()).ToList();
    }
}