using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IDoctorRoomRepository : IGenericRepository<DoctorRoom>
{
    Task<bool> HasActiveAssignmentAsync(
        int doctorId, int roomId, int? excludedAssignmentId, CancellationToken cancellationToken);
}
