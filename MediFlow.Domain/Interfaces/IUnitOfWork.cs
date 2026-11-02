namespace MediFlow.Domain.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}