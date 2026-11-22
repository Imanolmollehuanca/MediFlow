using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Commands;

public record CreateAppointmentCommand(CreateAppointmentDto Appointment)
    : IRequest<AppointmentDto>;

public class CreateAppointmentCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
{
    public async Task<AppointmentDto> Handle(
        CreateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Appointment;

        if (dto.PatientId <= 0 || dto.DoctorId <= 0)
            throw new ArgumentException(
                "Los IDs del paciente y médico deben ser válidos.");

        if (dto.StartDateTime >= dto.EndDateTime)
            throw new ArgumentException(
                "La fecha de inicio debe ser anterior a la fecha de fin.");

        if (dto.StartDateTime <= DateTime.UtcNow)
            throw new ArgumentException(
                "La cita debe programarse para una fecha futura.");

        if (dto.StartDateTime.Date != dto.EndDateTime.Date)
            throw new ArgumentException(
                "La cita debe comenzar y terminar el mismo día.");

        var patient = await unitOfWork.Patients
            .GetByIdAsync(dto.PatientId);

        if (patient is null)
            throw new KeyNotFoundException("El paciente no existe.");

        if (!patient.IsActive)
            throw new InvalidOperationException(
                "El paciente está inactivo.");

        var doctor = await unitOfWork.Doctors
            .GetByIdAsync(dto.DoctorId);

        if (doctor is null)
            throw new KeyNotFoundException("El médico no existe.");

        if (!doctor.IsActive)
            throw new InvalidOperationException(
                "El médico está inactivo.");

        var schedules = await unitOfWork.Schedules.GetAllAsync();

        var dayOfWeek = (short)dto.StartDateTime.DayOfWeek;
        var startTime = TimeOnly.FromDateTime(dto.StartDateTime);
        var endTime = TimeOnly.FromDateTime(dto.EndDateTime);

        var durationMinutes =
            (dto.EndDateTime - dto.StartDateTime).TotalMinutes;

        var validSchedule = schedules.Any(s =>
            s.DoctorId == dto.DoctorId &&
            s.IsActive &&
            s.DayOfWeek == dayOfWeek &&
            startTime >= s.StartTime &&
            endTime <= s.EndTime &&
            durationMinutes == s.SlotDurationMinutes &&
            (startTime - s.StartTime).TotalMinutes %
                s.SlotDurationMinutes == 0);

        if (!validSchedule)
            throw new InvalidOperationException(
                "La cita no corresponde a un horario o turno disponible del médico.");

        var appointments = await unitOfWork.Appointments.GetAllAsync();

        var overlap = appointments.Any(a =>
            a.Status != "Cancelled" &&
            a.StartDateTime < dto.EndDateTime &&
            a.EndDateTime > dto.StartDateTime &&
            (a.DoctorId == dto.DoctorId ||
             a.PatientId == dto.PatientId));

        if (overlap)
            throw new InvalidOperationException(
                "El médico o paciente ya tiene una cita en ese horario.");

        var appointment = dto.ToEntity();

        await unitOfWork.Appointments.AddAsync(appointment);
        await unitOfWork.SaveChangesAsync();

        return new AppointmentDto(appointment);
    }
}