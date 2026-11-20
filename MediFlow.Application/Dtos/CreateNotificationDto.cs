namespace MediFlow.Application.Dtos;

public record CreateNotificationDto(
    int PatientId, int? AppointmentId,
    string Title, string Message, string Type = "Reminder");