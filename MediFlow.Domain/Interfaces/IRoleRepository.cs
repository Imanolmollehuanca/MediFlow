using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int roleId, CancellationToken ct = default);
    Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, int? excludeRoleId, CancellationToken ct = default);
    Task<bool> HasUsersAsync(int roleId, CancellationToken ct = default);
    Task AddAsync(Role role, CancellationToken ct = default);
    void Remove(Role role);
}