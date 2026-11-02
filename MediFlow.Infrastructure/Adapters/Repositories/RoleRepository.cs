using MediFlow.Domain.Entities;
using MediFlow.Domain.Interfaces;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class RoleRepository(MediFlowContext context) : IRoleRepository
{
    public Task<Role?> GetByIdAsync(int roleId, CancellationToken ct = default) =>
        context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId, ct);

    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken ct = default) =>
        await context.Roles.AsNoTracking().OrderBy(r => r.RoleId).ToListAsync(ct);

    public Task<bool> ExistsByNameAsync(string name, int? excludeRoleId, CancellationToken ct = default) =>
        context.Roles.AnyAsync(r => r.Name == name && (excludeRoleId == null || r.RoleId != excludeRoleId), ct);

    public Task<bool> HasUsersAsync(int roleId, CancellationToken ct = default) =>
        context.UserRoles.AnyAsync(ur => ur.RoleId == roleId, ct);

    public async Task AddAsync(Role role, CancellationToken ct = default) =>
        await context.Roles.AddAsync(role, ct);

    public void Remove(Role role) => context.Roles.Remove(role);
}