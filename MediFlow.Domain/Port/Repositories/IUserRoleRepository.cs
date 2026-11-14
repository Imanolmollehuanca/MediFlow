using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<UserRole?> GetByIdsAsync(int userId, int roleId);
}