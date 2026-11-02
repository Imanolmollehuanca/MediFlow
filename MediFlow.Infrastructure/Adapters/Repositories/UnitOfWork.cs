using MediFlow.Domain.Interfaces;
using MediFlow.Infrastructure.Context;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class UnitOfWork(
    MediFlowContext context,
    IUserRepository users,
    IRoleRepository roles,
    IUserRoleRepository userRoles) : IUnitOfWork
{
    public IUserRepository Users => users;
    public IRoleRepository Roles => roles;
    public IUserRoleRepository UserRoles => userRoles;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}