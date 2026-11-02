
using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Commands;

public record UpdateAppointmentCommand(int AppointmentId, int PatientId, int DoctorId, DateTime StartDateTime,
    DateTime EndDateTime, string Status, string? Reason ) : IRequest<bool>;

internal sealed class UpdateAppointmentCommandHandler
    : IRequestHandler<UpdateAppointmentCommand, bool>
{
    private readonly IGenericRepository<Appointment> _appointments;
    private readonly IGenericRepository<Patient> _patients;
    private readonly IGenericRepository<Doctor> _doctors;
    private readonly IGenericRepository<Schedule> _schedules;

    public UpdateAppointmentCommandHandler(
        IGenericRepository<Appointment> appointments,
        IGenericRepository<Patient> patients,
        IGenericRepository<Doctor> doctors,
        IGenericRepository<Schedule> schedules)
    {
        _appointments = appointments;
        _patients = patients;
        _doctors = doctors;
        _schedules = schedules;
    }

    public async Task<bool> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointments.GetByIdAsync(request.AppointmentId);

        if (appointment is null)
            return false;

        var allowedStatuses = new[]
        {
            "Pending", "Confirmed", "Completed", "Cancelled"
        };

        if (!allowedStatuses.Contains(request.Status))
            throw new ArgumentException("Estado inválido. Use Pending, Confirmed, Completed o Cancelled.");

        if (request.Status != "Cancelled")
        {
            await AppointmentValidation.ValidateAsync(_appointments, _patients, _doctors, _schedules, request.PatientId, request.DoctorId, request.StartDateTime, request.EndDateTime, request.AppointmentId);
        }
        else if (request.PatientId != appointment.PatientId ||
                 request.DoctorId != appointment.DoctorId ||
                 request.StartDateTime != appointment.StartDateTime ||
                 request.EndDateTime != appointment.EndDateTime)
        {
            throw new ArgumentException("Para cancelar una cita no modifique el paciente, " + "médico ni las fechas.");
        }

        appointment.PatientId = request.PatientId;
        appointment.DoctorId = request.DoctorId;
        appointment.StartDateTime = request.StartDateTime;
        appointment.EndDateTime = request.EndDateTime;
        appointment.Status = request.Status;
        appointment.Reason = request.Reason;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _appointments.UpdateAsync(appointment);
        return true;
    }
}
