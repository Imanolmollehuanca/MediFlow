using MediFlow.Domain.Entities;
using MediFlow.Domain.Interfaces;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class UserRepository(MediFlowContext context) : IUserRepository
{
    private IQueryable<User> UsersWithRoles =>
        context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);

    public Task<User?> GetByIdAsync(int userId, CancellationToken ct = default) =>
        UsersWithRoles.FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default) =>
        await UsersWithRoles.AsNoTracking().OrderBy(u => u.UserId).ToListAsync(ct);

    public Task<bool> ExistsByEmailAsync(string email, int? excludeUserId, CancellationToken ct = default) =>
        context.Users.AnyAsync(u => u.Email == email && (excludeUserId == null || u.UserId != excludeUserId), ct);

    public Task<bool> ExistsByUsernameAsync(string username, int? excludeUserId, CancellationToken ct = default) =>
        context.Users.AnyAsync(u => u.Username == username && (excludeUserId == null || u.UserId != excludeUserId), ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await context.Users.AddAsync(user, ct);

    public void Remove(User user) => context.Users.Remove(user);
}