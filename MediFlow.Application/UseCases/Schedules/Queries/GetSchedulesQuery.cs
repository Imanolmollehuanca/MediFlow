using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Queries;

public record GetSchedulesQuery(int? DoctorId = null) : IRequest<IEnumerable<ScheduleDto>>;

internal sealed class GetSchedulesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetSchedulesQuery, IEnumerable<ScheduleDto>>
{
    public async Task<IEnumerable<ScheduleDto>> Handle(
        GetSchedulesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DoctorId is <= 0)
            throw new ArgumentException("El filtro de médico debe tener un ID positivo.");

        var schedules = await unitOfWork.Schedules.GetSchedulesAsync(
            request.DoctorId, cancellationToken);

        return schedules.Select(ScheduleDto.FromEntity).ToList();
    }
}
