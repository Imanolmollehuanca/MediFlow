using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record NotificationDto(
    int NotificationId, int PatientId, int? AppointmentId, string Title,
    string Message, string Type, bool IsRead, DateTime CreatedAt, DateTime? ReadAt)
{
    public static NotificationDto FromEntity(Notification notification)
        => new(
            notification.NotificationId, notification.PatientId, notification.AppointmentId, notification.Title,
            notification.Message, notification.Type, notification.IsRead, notification.CreatedAt, notification.ReadAt
        );
}