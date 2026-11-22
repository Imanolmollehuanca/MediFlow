using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Queries;

public record GetDoctorAvailabilityQuery(int DoctorId, DateOnly Date)
    : IRequest<IEnumerable<ScheduleAvailabilityDto>>;

internal sealed class GetDoctorAvailabilityQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetDoctorAvailabilityQuery, IEnumerable<ScheduleAvailabilityDto>>
{
    public async Task<IEnumerable<ScheduleAvailabilityDto>> Handle(
        GetDoctorAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        if (request.DoctorId <= 0)
            throw new ArgumentException("El ID del médico debe ser positivo.");

        if (request.Date < DateOnly.FromDateTime(now))
            throw new ArgumentException("La fecha de consulta no puede ser anterior al día actual en UTC.");

        var doctor = await unitOfWork.Doctors.GetByIdAsync(request.DoctorId)
            ?? throw new KeyNotFoundException("El médico no existe.");

        if (!doctor.IsActive)
            return [];

        var dayOfWeek = (short)request.Date.DayOfWeek;
        var schedules = (await unitOfWork.Schedules.GetSchedulesAsync(request.DoctorId, cancellationToken))
            .Where(schedule => schedule.IsActive && schedule.DayOfWeek == dayOfWeek)
            .OrderBy(schedule => schedule.StartTime)
            .ThenBy(schedule => schedule.ScheduleId)
            .ToList();

        if (schedules.Count == 0)
            return [];

        for (var i = 0; i < schedules.Count; i++)
        {
            var schedule = schedules[i];
            if (schedule.StartTime >= schedule.EndTime || schedule.SlotDurationMinutes <= 0
                || TimeSpan.FromMinutes(schedule.SlotDurationMinutes) > schedule.EndTime - schedule.StartTime
                || (i > 0 && schedule.StartTime < schedules[i - 1].EndTime))
            {
                throw new InvalidOperationException("Los horarios activos del médico no son válidos para calcular disponibilidad.");
            }
        }

        var roomIds = await unitOfWork.DoctorRooms.GetActiveRoomIdsAsync(request.DoctorId, cancellationToken);
        var roomSchedules = (await unitOfWork.Schedules.GetRoomSchedulesAsync(
            request.DoctorId, dayOfWeek, cancellationToken)).ToList();
        var rangeStart = request.Date.ToDateTime(schedules[0].StartTime, DateTimeKind.Utc);
        var rangeEnd = request.Date.ToDateTime(schedules[^1].EndTime, DateTimeKind.Utc);
        var appointments = (await unitOfWork.Appointments.GetBlockingAppointmentsAsync(
            request.DoctorId, rangeStart, rangeEnd, roomIds, cancellationToken)).ToList();

        List<ScheduleAvailabilityDto> slots = [];
        foreach (var schedule in schedules)
        {
            var duration = TimeSpan.FromMinutes(schedule.SlotDurationMinutes);
            var end = request.Date.ToDateTime(schedule.EndTime, DateTimeKind.Utc);
            for (var start = request.Date.ToDateTime(schedule.StartTime, DateTimeKind.Utc);
                 start <= end - duration;
                 start += duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var slot = ScheduleAvailabilityDto.FromEntity(schedule, start, roomIds);
                if (slot.StartDateTime <= now
                    || appointments.Any(appointment => appointment.StartDateTime < slot.EndDateTime
                        && appointment.EndDateTime > slot.StartDateTime)
                    || roomSchedules.Any(other => TimeOnly.FromDateTime(slot.StartDateTime) < other.EndTime
                        && TimeOnly.FromDateTime(slot.EndDateTime) > other.StartTime))
                {
                    continue;
                }

                slots.Add(slot);
            }
        }

        return slots;
    }
}
