using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface ISpecialtyRepository : IGenericRepository<Specialty>
{
    Task<bool> ExistsByNameAsync(
        string name,
        int? excludedSpecialtyId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasActiveDoctorsAsync(
        int specialtyId,
        CancellationToken cancellationToken = default);
}
