using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Notifications.Queries;

public record GetAllNotificationsQuery()
    : IRequest<List<NotificationDto>>;

public class GetAllNotificationsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllNotificationsQuery, List<NotificationDto>>
{
    public async Task<List<NotificationDto>> Handle(
        GetAllNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications = await unitOfWork.Notifications.GetAllAsync();

        return notifications
            .Select(NotificationDto.FromEntity)
            .ToList();
    }
}