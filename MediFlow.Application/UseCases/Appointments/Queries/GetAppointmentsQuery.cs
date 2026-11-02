using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Queries;

public record GetAppointmentsQuery()
    : IRequest<IEnumerable<AppointmentDto>>;

internal sealed class GetAppointmentsQueryHandler
    : IRequestHandler<GetAppointmentsQuery, IEnumerable<AppointmentDto>>
{
    private readonly IGenericRepository<Appointment> _repository;

    public GetAppointmentsQueryHandler(
        IGenericRepository<Appointment> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AppointmentDto>> Handle(
        GetAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments = await _repository.GetAllAsync();

        return appointments.Select(a => new AppointmentDto(a.AppointmentId, a.PatientId, a.DoctorId, a.StartDateTime, a.EndDateTime, a.Status, a.Reason, a.CreatedAt, a.UpdatedAt
        )).ToList();
    }
}