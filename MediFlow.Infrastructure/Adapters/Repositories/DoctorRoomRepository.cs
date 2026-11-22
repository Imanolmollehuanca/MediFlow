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

    public Task<bool> HasScheduleConflictAsync(
        int doctorId, int roomId, int? excludedAssignmentId = null,
        CancellationToken cancellationToken = default)
    {
        var conflicts =
            from schedule in Context.Schedules
            from otherAssignment in DbSet
            join otherSchedule in Context.Schedules on otherAssignment.DoctorId equals otherSchedule.DoctorId
            where schedule.IsActive && schedule.DoctorId == doctorId
                && otherAssignment.IsActive && otherAssignment.RoomId == roomId
                && otherAssignment.DoctorId != doctorId
                && (!excludedAssignmentId.HasValue || otherAssignment.DoctorRoomId != excludedAssignmentId.Value)
                && otherSchedule.IsActive && otherSchedule.DayOfWeek == schedule.DayOfWeek
                && schedule.StartTime < otherSchedule.EndTime && schedule.EndTime > otherSchedule.StartTime
            select otherAssignment.DoctorRoomId;

        return conflicts.AnyAsync(cancellationToken);
    }
}
