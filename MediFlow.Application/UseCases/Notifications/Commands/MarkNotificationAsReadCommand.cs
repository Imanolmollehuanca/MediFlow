using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Notifications.Commands;

public record MarkNotificationAsReadCommand(int NotificationId)
    : IRequest<NotificationDto?>;

public class MarkNotificationAsReadCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<MarkNotificationAsReadCommand, NotificationDto?>
{
    public async Task<NotificationDto?> Handle(
        MarkNotificationAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await unitOfWork.Notifications
            .GetByIdAsync(request.NotificationId);

        if (notification is null)
            return null;

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            await unitOfWork.Notifications.UpdateAsync(notification);
            await unitOfWork.SaveChangesAsync();
        }

        return NotificationDto.FromEntity(notification);
    }
}