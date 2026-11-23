using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Queries;

public record GetScheduleByIdQuery(int ScheduleId) : IRequest<ScheduleDto?>;

internal sealed class GetScheduleByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetScheduleByIdQuery, ScheduleDto?>
{
    public async Task<ScheduleDto?> Handle(
        GetScheduleByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.ScheduleId <= 0)
            return null;

        var schedule = await unitOfWork.Schedules.GetByIdAsync(request.ScheduleId);

        return schedule is null ? null : ScheduleDto.FromEntity(schedule);
    }
}
