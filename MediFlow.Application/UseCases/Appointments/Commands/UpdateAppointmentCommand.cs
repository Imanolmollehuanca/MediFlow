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
            throw new ArgumentException(
                "Estado inválido. Use Scheduled, Confirmed, Completed o Cancelled.");

        if (dto.PatientId != appointment.PatientId ||
            dto.DoctorId != appointment.DoctorId ||
            dto.StartDateTime != appointment.StartDateTime ||
            dto.EndDateTime != appointment.EndDateTime)
        {
            throw new InvalidOperationException(
                "No se permite cambiar el paciente, médico ni horario mediante esta operación.");
        }

        if (appointment.Status is "Completed" or "Cancelled" &&
            dto.Status != appointment.Status)
        {
            throw new InvalidOperationException(
                "No se puede cambiar el estado de una cita finalizada o cancelada.");
        }

        dto.ApplyTo(appointment);

        await unitOfWork.Appointments.UpdateAsync(appointment);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}