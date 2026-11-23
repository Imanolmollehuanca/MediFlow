using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class SpecialtyRepository(MediFlowContext context)
    : GenericRepository<Specialty>(context), ISpecialtyRepository
{
    public Task<bool> ExistsByNameAsync(
        string name,
        int? excludedSpecialtyId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToLowerInvariant();

        return DbSet.AnyAsync(specialty =>
            (!excludedSpecialtyId.HasValue ||
                specialty.SpecialtyId != excludedSpecialtyId.Value) &&
            specialty.Name.Trim().ToLower() == normalizedName,
            cancellationToken);
    }

    public Task<bool> HasActiveDoctorsAsync(
        int specialtyId,
        CancellationToken cancellationToken = default)
    {
        return Context.Doctors.AnyAsync(doctor =>
            doctor.SpecialtyId == specialtyId && doctor.IsActive,
            cancellationToken);
    }
}
