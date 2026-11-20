using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Notifications.Commands;

public record DeleteNotificationCommand(int NotificationId)
    : IRequest<bool>;

public class DeleteNotificationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteNotificationCommand, bool>
{
    public async Task<bool> Handle(
        DeleteNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await unitOfWork.Notifications
            .GetByIdAsync(request.NotificationId);

        if (notification is null)
            return false;

        await unitOfWork.Notifications.DeleteAsync(notification);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}