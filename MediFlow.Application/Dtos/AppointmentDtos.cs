namespace MediFlow.Application.Dtos;

public record AppointmentDto(int AppointmentId, int PatientId, int DoctorId,
    DateTime StartDateTime, DateTime EndDateTime, string Status, string? Reason,
    DateTime CreatedAt, DateTime? UpdatedAt );

public record CreateAppointmentDto(int PatientId, int DoctorId,
    DateTime StartDateTime, DateTime EndDateTime, string? Reason );

public record UpdateAppointmentDto(int PatientId, int DoctorId, 
    DateTime StartDateTime, DateTime EndDateTime, string Status, string? Reason );