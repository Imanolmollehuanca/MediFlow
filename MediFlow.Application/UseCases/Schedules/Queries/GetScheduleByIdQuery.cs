using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Queries;

public record GetScheduleByIdQuery(int ScheduleId) : IRequest<ScheduleDto?>;
internal sealed class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ScheduleDto?>
{
    private readonly IGenericRepository<Schedule> _repository;
    public GetScheduleByIdQueryHandler(IGenericRepository<Schedule> repository)
    {
        _repository = repository;
    }

    public async Task<ScheduleDto?> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);

        if (schedule is null)
            return null;

        return new ScheduleDto(
            schedule.ScheduleId,
            schedule.DoctorId,
            schedule.DayOfWeek,
            schedule.StartTime,
            schedule.EndTime,
            schedule.SlotDurationMinutes,
            schedule.IsActive
        );
    }
}