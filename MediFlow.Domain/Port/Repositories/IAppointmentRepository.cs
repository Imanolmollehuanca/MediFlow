using MediFlow.Domain.Entities;

namespace MediFlow.Domain.Port.Repositories;

public interface IAppointmentRepository
    : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetBlockingAppointmentsAsync(
        int doctorId,
        DateTime startDateTime,
        DateTime endDateTime,
        IReadOnlyList<int> roomIds,
        CancellationToken cancellationToken = default);
}
