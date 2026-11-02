using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Interfaces;

public interface IUserRoleRepository
{
    Task<UserRole?> GetAsync(int userId, int roleId, CancellationToken ct = default);
    Task AddAsync(UserRole userRole, CancellationToken ct = default);
    void Remove(UserRole userRole);
}