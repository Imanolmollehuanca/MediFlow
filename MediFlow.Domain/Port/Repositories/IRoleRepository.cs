using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
}