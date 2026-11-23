using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class AppointmentRepository(MediFlowContext context)
    : GenericRepository<Appointment>(context), IAppointmentRepository
{
    public async Task<IEnumerable<Appointment>> GetBlockingAppointmentsAsync(
        int doctorId,
        DateTime startDateTime,
        DateTime endDateTime,
        IReadOnlyList<int> roomIds,
        CancellationToken cancellationToken = default)
    {
        var roomAssignments = Context.DoctorRooms
            .Where(assignment => assignment.IsActive && roomIds.Contains(assignment.RoomId));

        return await DbSet.AsNoTracking()
            .Where(appointment => appointment.Status != "Cancelled"
                && appointment.StartDateTime < endDateTime && appointment.EndDateTime > startDateTime
                && (appointment.DoctorId == doctorId
                    || roomAssignments.Any(assignment => assignment.DoctorId == appointment.DoctorId)))
            .OrderBy(appointment => appointment.StartDateTime)
            .ToListAsync(cancellationToken);
    }
}
