using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<bool> ExistsByRoomNumberAsync(
        string roomNumber,
        int? excludedRoomId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasActiveDoctorAssignmentsAsync(
        int roomId,
        CancellationToken cancellationToken = default);
}
