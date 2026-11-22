using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Commands;

public record UpdateAppointmentCommand(
    int AppointmentId,
    UpdateAppointmentDto Appointment
) : IRequest<bool>;

public class UpdateAppointmentCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateAppointmentCommand, bool>
{
    private static readonly string[] AllowedStatuses =
    [
        "Scheduled",
        "Confirmed",
        "Completed",
        "Cancelled"
    ];

    public async Task<bool> Handle(
        UpdateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = await unitOfWork.Appointments
            .GetByIdAsync(request.AppointmentId);

        if (appointment is null)
            return false;

        var dto = request.Appointment;

        if (!AllowedStatuses.Contains(dto.Status))
            throw new ArgumentException("Estado de cita inválido.");

        if (dto.PatientId != appointment.PatientId ||
            dto.DoctorId != appointment.DoctorId)
            throw new InvalidOperationException(
                "No se permite cambiar el paciente ni el médico.");

        if (appointment.Status is "Completed" or "Cancelled")
            throw new InvalidOperationException(
                "No se puede modificar una cita finalizada o cancelada.");

        bool changingSchedule =
            dto.StartDateTime != appointment.StartDateTime ||
            dto.EndDateTime != appointment.EndDateTime;

        if (changingSchedule)
        {
            if (dto.StartDateTime.Kind != DateTimeKind.Utc ||
                dto.EndDateTime.Kind != DateTimeKind.Utc)
                throw new ArgumentException(
                    "Las fechas deben enviarse en UTC (con Z).");

            if (dto.StartDateTime >= dto.EndDateTime)
                throw new ArgumentException(
                    "La fecha de inicio debe ser anterior a la de fin.");

            if (dto.StartDateTime <= DateTime.UtcNow)
                throw new ArgumentException(
                    "La nueva fecha debe ser futura.");

            if (dto.StartDateTime.Date != dto.EndDateTime.Date)
                throw new ArgumentException(
                    "La cita debe comenzar y terminar el mismo día.");

            var schedules = await unitOfWork.Schedules.GetAllAsync();

            var dayOfWeek = (short)dto.StartDateTime.DayOfWeek;
            var startTime = TimeOnly.FromDateTime(dto.StartDateTime);
            var endTime = TimeOnly.FromDateTime(dto.EndDateTime);

            var durationMinutes =
                (dto.EndDateTime - dto.StartDateTime).TotalMinutes;

            var validSchedule = schedules.Any(s =>
                s.DoctorId == dto.DoctorId &&
                s.IsActive &&
                s.SlotDurationMinutes > 0 &&
                s.DayOfWeek == dayOfWeek &&
                startTime >= s.StartTime &&
                endTime <= s.EndTime &&
                durationMinutes == s.SlotDurationMinutes &&
                (startTime - s.StartTime).TotalMinutes %
                    s.SlotDurationMinutes == 0);

            if (!validSchedule)
                throw new InvalidOperationException(
                    "El nuevo horario no está disponible para el médico.");

            var appointments =
                await unitOfWork.Appointments.GetAllAsync();

            var overlap = appointments.Any(a =>
                a.AppointmentId != appointment.AppointmentId &&
                a.Status != "Cancelled" &&
                a.StartDateTime < dto.EndDateTime &&
                a.EndDateTime > dto.StartDateTime &&
                (a.DoctorId == dto.DoctorId ||
                 a.PatientId == dto.PatientId));

            if (overlap)
                throw new InvalidOperationException(
                    "El médico o paciente ya tiene una cita en ese horario.");
        }

        dto.ApplyTo(appointment);

        await unitOfWork.Appointments.UpdateAsync(appointment);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}
