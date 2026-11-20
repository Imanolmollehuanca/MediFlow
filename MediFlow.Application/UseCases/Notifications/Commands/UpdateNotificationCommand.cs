using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Notifications.Commands;

public record UpdateNotificationCommand(
    int NotificationId,
    UpdateNotificationDto Dto)
    : IRequest<NotificationDto?>;

public class UpdateNotificationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateNotificationCommand, NotificationDto?>
{
    public async Task<NotificationDto?> Handle(
        UpdateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await unitOfWork.Notifications
            .GetByIdAsync(request.NotificationId);

        if (notification is null)
            return null;

        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("El título es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Message))
            throw new ArgumentException("El mensaje es obligatorio.");

        notification.Title = dto.Title.Trim();
        notification.Message = dto.Message.Trim();
        notification.Type = string.IsNullOrWhiteSpace(dto.Type)
            ? "Reminder"
            : dto.Type.Trim();

        await unitOfWork.Notifications.UpdateAsync(notification);
        await unitOfWork.SaveChangesAsync();

        return NotificationDto.FromEntity(notification);
    }
}