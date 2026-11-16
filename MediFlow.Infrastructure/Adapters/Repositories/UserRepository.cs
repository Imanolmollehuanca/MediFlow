using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class UserRepository(MediFlowContext context)
    : GenericRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await DbSet
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await DbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetWithRolesAsync(int userId)
    {
        return await DbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}