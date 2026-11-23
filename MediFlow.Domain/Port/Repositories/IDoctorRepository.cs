using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<bool> ExistsByLicenseNumberAsync(
        string licenseNumber,
        int? excludedDoctorId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        string email,
        int? excludedDoctorId = null,
        CancellationToken cancellationToken = default);
}
