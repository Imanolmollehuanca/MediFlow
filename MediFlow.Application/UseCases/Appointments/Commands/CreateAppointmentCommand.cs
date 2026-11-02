using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Commands;

public record CreateAppointmentCommand(int PatientId, int DoctorId,
    DateTime StartDateTime, DateTime EndDateTime, string? Reason ) : IRequest<int>;

internal sealed class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, int>
{
    private readonly IGenericRepository<Appointment> _appointments;
    private readonly IGenericRepository<Patient> _patients;
    private readonly IGenericRepository<Doctor> _doctors;
    private readonly IGenericRepository<Schedule> _schedules;

    public CreateAppointmentCommandHandler(
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

    public async Task<int> Handle(
        CreateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        await AppointmentValidation.ValidateAsync(_appointments, _patients, _doctors, _schedules, request.PatientId, request.DoctorId, request.StartDateTime, request.EndDateTime);

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            Reason = request.Reason,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _appointments.AddAsync(appointment);
        return appointment.AppointmentId;
    }
}