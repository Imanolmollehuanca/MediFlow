using MediFlow.Domain.Entities;



namespace MediFlow.Domain.Interfaces;

public interface IUserRepository
{
    
    Task<User?> GetByIdAsync(int userId, CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, int? excludeUserId, CancellationToken ct = default);
    Task<bool> ExistsByUsernameAsync(string username, int? excludeUserId, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    void Remove(User user);
}