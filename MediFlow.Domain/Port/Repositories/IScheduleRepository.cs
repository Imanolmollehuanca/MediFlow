using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IScheduleRepository : IGenericRepository<Schedule>
{
    Task<IEnumerable<Schedule>> GetSchedulesAsync(
        int? doctorId = null,
        CancellationToken cancellationToken = default);
}
