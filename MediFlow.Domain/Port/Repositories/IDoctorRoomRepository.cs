using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IDoctorRoomRepository : IGenericRepository<DoctorRoom>
{
    Task<IEnumerable<DoctorRoom>> GetAssignmentsAsync(
        int? doctorId = null,
        int? roomId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasActiveAssignmentAsync(
        int doctorId, int roomId, int? excludedAssignmentId, CancellationToken cancellationToken);

    Task<bool> HasScheduleConflictAsync(
        int doctorId, int roomId, int? excludedAssignmentId = null,
        CancellationToken cancellationToken = default);
}
