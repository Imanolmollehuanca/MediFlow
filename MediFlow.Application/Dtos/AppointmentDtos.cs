using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record AppointmentDto(
    int AppointmentId, int PatientId, int DoctorId, DateTime StartDateTime,
    DateTime EndDateTime, string Status, string? Reason, DateTime CreatedAt, DateTime? UpdatedAt
)
{
    public AppointmentDto(Appointment appointment) : this(
        appointment.AppointmentId, appointment.PatientId, appointment.DoctorId, appointment.StartDateTime, appointment.EndDateTime,
        appointment.Status, appointment.Reason, appointment.CreatedAt, appointment.UpdatedAt
    )
    {
    }
}

public record CreateAppointmentDto(
    int PatientId, int DoctorId,
    DateTime StartDateTime, DateTime EndDateTime, string? Reason
)
{
    public Appointment ToEntity()
    {
        return new Appointment
        {
            PatientId = PatientId,
            DoctorId = DoctorId,
            StartDateTime = StartDateTime,
            EndDateTime = EndDateTime,
            Status = "Scheduled",
            Reason = Reason,
            CreatedAt = DateTime.UtcNow
        };
    }
}

public record UpdateAppointmentDto(
    int PatientId, int DoctorId, DateTime StartDateTime,
    DateTime EndDateTime, string Status, string? Reason
)
{
    public void ApplyTo(Appointment appointment)
    {
        appointment.PatientId = PatientId;
        appointment.DoctorId = DoctorId;
        appointment.StartDateTime = StartDateTime;
        appointment.EndDateTime = EndDateTime;
        appointment.Status = Status;
        appointment.Reason = Reason;
        appointment.UpdatedAt = DateTime.UtcNow;
    }
}