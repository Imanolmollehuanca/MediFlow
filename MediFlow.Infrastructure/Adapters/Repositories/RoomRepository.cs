using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class RoomRepository(MediFlowContext context)
    : GenericRepository<Room>(context), IRoomRepository
{
    public Task<bool> ExistsByRoomNumberAsync(
        string roomNumber,
        int? excludedRoomId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedRoomNumber = roomNumber.Trim().ToLowerInvariant();

        return DbSet.AnyAsync(room =>
            (!excludedRoomId.HasValue || room.RoomId != excludedRoomId.Value) &&
            room.RoomNumber.Trim().ToLower() == normalizedRoomNumber,
            cancellationToken);
    }

    public Task<bool> HasActiveDoctorAssignmentsAsync(
        int roomId,
        CancellationToken cancellationToken = default)
    {
        return Context.DoctorRooms.AnyAsync(assignment =>
            assignment.RoomId == roomId && assignment.IsActive,
            cancellationToken);
    }
}
