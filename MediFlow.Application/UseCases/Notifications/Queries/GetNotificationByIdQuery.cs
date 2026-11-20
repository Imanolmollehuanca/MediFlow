using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Notifications.Queries;

public record GetNotificationByIdQuery(int NotificationId)
    : IRequest<NotificationDto?>;

public class GetNotificationByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
{
    public async Task<NotificationDto?> Handle(
        GetNotificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var notification = await unitOfWork.Notifications
            .GetByIdAsync(request.NotificationId);

        return notification is null
            ? null
            : NotificationDto.FromEntity(notification);
    }
}