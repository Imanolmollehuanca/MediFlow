using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class DoctorRoomRepository(MediFlowContext context)
    : GenericRepository<DoctorRoom>(context), IDoctorRoomRepository
{
    public Task<bool> HasActiveAssignmentAsync(
        int doctorId, int roomId, int? excludedAssignmentId, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(a => a.IsActive && a.DoctorId == doctorId && a.RoomId == roomId
            && a.DoctorRoomId != excludedAssignmentId, cancellationToken);
    }
}
