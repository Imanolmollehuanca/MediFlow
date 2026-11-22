using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Queries;

public record GetAppointmentByIdQuery(int AppointmentId)
    : IRequest<AppointmentDto?>;

public class GetAppointmentByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    public async Task<AppointmentDto?> Handle(
        GetAppointmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var appointment = await unitOfWork.Appointments
            .GetByIdAsync(request.AppointmentId);

        return appointment is null
            ? null
            : new AppointmentDto(appointment);
    }
}