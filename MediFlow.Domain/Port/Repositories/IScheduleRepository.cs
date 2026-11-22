using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IScheduleRepository : IGenericRepository<Schedule>
{
    Task<IEnumerable<Schedule>> GetSchedulesAsync(
        int? doctorId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasOverlappingScheduleAsync(
        int doctorId,
        short dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int? excludedScheduleId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasRoomConflictAsync(
        int doctorId,
        short dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int? excludedScheduleId = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Schedule>> GetRoomSchedulesAsync(
        int doctorId, short dayOfWeek, CancellationToken cancellationToken = default);
}
