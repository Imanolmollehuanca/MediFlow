using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<bool> ExistsByDocumentAsync(string documentType, string documentNumber, int? excludePatientId = null);

    Task<IEnumerable<Patient>> SearchAsync(string? documentNumber, string? name, bool? isActive);
}