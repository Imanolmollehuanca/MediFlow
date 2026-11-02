using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Queries;

public record GetSchedulesQuery() : IRequest<IEnumerable<ScheduleDto>>;

internal sealed class GetSchedulesQueryHandler : IRequestHandler<GetSchedulesQuery, IEnumerable<ScheduleDto>>
{
    private readonly IGenericRepository<Schedule> _repository;

    public GetSchedulesQueryHandler(IGenericRepository<Schedule> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ScheduleDto>> Handle(GetSchedulesQuery request, CancellationToken cancellationToken)
    {
        var schedules = await _repository.GetAllAsync();

        return schedules.Select(s => new ScheduleDto(
            s.ScheduleId,
            s.DoctorId,
            s.DayOfWeek,
            s.StartTime,
            s.EndTime,
            s.SlotDurationMinutes,
            s.IsActive
        )).ToList();
    }
}