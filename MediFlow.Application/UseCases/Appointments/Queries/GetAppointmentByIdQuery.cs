using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Queries;

public record GetAppointmentByIdQuery(int AppointmentId) : IRequest<AppointmentDto?>;

internal sealed class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    private readonly IGenericRepository<Appointment> _repository;

    public GetAppointmentByIdQueryHandler(
        IGenericRepository<Appointment> repository)
    {
        _repository = repository;
    }

    public async Task<AppointmentDto?> Handle(
        GetAppointmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetByIdAsync(request.AppointmentId);

        if (appointment is null)
            return null;

        return new AppointmentDto(appointment.AppointmentId, appointment.PatientId, appointment.DoctorId, appointment.StartDateTime, appointment.EndDateTime, appointment.Status, appointment.Reason, appointment.CreatedAt, appointment.UpdatedAt
        );
    }
}