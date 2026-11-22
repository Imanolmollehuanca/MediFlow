using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class ScheduleRepository(MediFlowContext context)
    : GenericRepository<Schedule>(context), IScheduleRepository
{
    public async Task<IEnumerable<Schedule>> GetSchedulesAsync(
        int? doctorId = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (doctorId.HasValue)
            query = query.Where(schedule => schedule.DoctorId == doctorId.Value);

        return await query
            .OrderBy(schedule => schedule.ScheduleId)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> HasOverlappingScheduleAsync(
        int doctorId,
        short dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int? excludedScheduleId = null,
        CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(schedule =>
            schedule.IsActive &&
            schedule.DoctorId == doctorId &&
            schedule.DayOfWeek == dayOfWeek &&
            (!excludedScheduleId.HasValue || schedule.ScheduleId != excludedScheduleId.Value) &&
            startTime < schedule.EndTime && endTime > schedule.StartTime,
            cancellationToken);
    }

    public Task<bool> HasRoomConflictAsync(
        int doctorId,
        short dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int? excludedScheduleId = null,
        CancellationToken cancellationToken = default)
    {
        var conflicts =
            from assignedRoom in Context.DoctorRooms
            join otherAssignment in Context.DoctorRooms on assignedRoom.RoomId equals otherAssignment.RoomId
            join otherSchedule in DbSet on otherAssignment.DoctorId equals otherSchedule.DoctorId
            where assignedRoom.IsActive && assignedRoom.DoctorId == doctorId
                && otherAssignment.IsActive && otherAssignment.DoctorId != doctorId
                && otherSchedule.IsActive && otherSchedule.DayOfWeek == dayOfWeek
                && (!excludedScheduleId.HasValue || otherSchedule.ScheduleId != excludedScheduleId.Value)
                && startTime < otherSchedule.EndTime && endTime > otherSchedule.StartTime
            select otherSchedule.ScheduleId;

        return conflicts.AnyAsync(cancellationToken);
    }
}
