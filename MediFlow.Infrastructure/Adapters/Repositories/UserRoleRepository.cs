using MediFlow.Domain.Entities;
using MediFlow.Domain.Interfaces;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class UserRoleRepository(MediFlowContext context) : IUserRoleRepository
{
    public Task<UserRole?> GetAsync(int userId, int roleId, CancellationToken ct = default) =>
        context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, ct);

    public async Task AddAsync(UserRole userRole, CancellationToken ct = default) =>
        await context.UserRoles.AddAsync(userRole, ct);

    public void Remove(UserRole userRole) => context.UserRoles.Remove(userRole);
}