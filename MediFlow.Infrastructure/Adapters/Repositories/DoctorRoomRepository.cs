using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class DoctorRoomRepository(MediFlowContext context)
    : GenericRepository<DoctorRoom>(context), IDoctorRoomRepository
{
    public async Task<IEnumerable<DoctorRoom>> GetAssignmentsAsync(
        int? doctorId = null,
        int? roomId = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (doctorId.HasValue)
            query = query.Where(assignment => assignment.DoctorId == doctorId.Value);

        if (roomId.HasValue)
            query = query.Where(assignment => assignment.RoomId == roomId.Value);

        return await query
            .OrderBy(assignment => assignment.DoctorRoomId)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasActiveAssignmentAsync(
        int doctorId, int roomId, int? excludedAssignmentId, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(a => a.IsActive && a.DoctorId == doctorId && a.RoomId == roomId
            && a.DoctorRoomId != excludedAssignmentId, cancellationToken);
    }
}
