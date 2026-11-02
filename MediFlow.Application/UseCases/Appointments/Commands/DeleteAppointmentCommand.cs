using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Commands;

public record DeleteAppointmentCommand(int AppointmentId) : IRequest<bool>;
internal sealed class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, bool>
{
    private readonly IGenericRepository<Appointment> _repository;

    public DeleteAppointmentCommandHandler(I­GenericRepository<Appointment> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetByIdAsync(request.AppointmentId);

        if (appointment is null ||
            appointment.Status == "Cancelled")
            return false;

        appointment.Status = "Cancelled";
        appointment.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(appointment);
        return true;
    }
}