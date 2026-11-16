using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class RoleRepository(MediFlowContext context)
    : GenericRepository<Role>(context), IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await DbSet
            .FirstOrDefaultAsync(r => r.Name == name);
    }
}