using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Queries;

public record GetAllAppointmentsQuery
    : IRequest<IEnumerable<AppointmentDto>>;

public class GetAllAppointmentsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentDto>>
{
    public async Task<IEnumerable<AppointmentDto>> Handle(
        GetAllAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments = await unitOfWork.Appointments.GetAllAsync();

        return appointments.Select(a => new AppointmentDto(a));
    }
}