using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(MediFlowContext context)
        : base(context)
    {
    }

    public async Task<UserRole?> GetByIdsAsync(int userId, int roleId)
    {
        return await DbSet.FirstOrDefaultAsync(ur =>
            ur.UserId == userId &&
            ur.RoleId == roleId);
    }
}
