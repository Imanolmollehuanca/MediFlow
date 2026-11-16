using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetWithRolesAsync(int userId);
}