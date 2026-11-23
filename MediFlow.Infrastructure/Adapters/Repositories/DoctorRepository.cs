using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class DoctorRepository(MediFlowContext context)
    : GenericRepository<Doctor>(context), IDoctorRepository
{
    public Task<bool> ExistsByLicenseNumberAsync(
        string licenseNumber,
        int? excludedDoctorId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedLicenseNumber = licenseNumber.Trim().ToLowerInvariant();

        return DbSet.AnyAsync(doctor =>
            (!excludedDoctorId.HasValue || doctor.DoctorId != excludedDoctorId.Value) &&
            doctor.LicenseNumber.Trim().ToLower() == normalizedLicenseNumber,
            cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(
        string email,
        int? excludedDoctorId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return DbSet.AnyAsync(doctor =>
            (!excludedDoctorId.HasValue || doctor.DoctorId != excludedDoctorId.Value) &&
            doctor.Email.Trim().ToLower() == normalizedEmail,
            cancellationToken);
    }
}
