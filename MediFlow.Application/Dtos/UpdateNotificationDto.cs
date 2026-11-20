namespace MediFlow.Application.Dtos;

public record UpdateNotificationDto(
    string Title, string Message, string Type);