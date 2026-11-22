using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Appointments.Commands;

public record DeleteAppointmentCommand(int AppointmentId)
    : IRequest<bool>;

public class DeleteAppointmentCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAppointmentCommand, bool>
{
    public async Task<bool> Handle(
        DeleteAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = await unitOfWork.Appointments
            .GetByIdAsync(request.AppointmentId);

        if (appointment is null)
            return false;

        if (appointment.Status == "Completed")
            throw new InvalidOperationException(
                "No se puede cancelar una cita completada.");

        if (appointment.Status == "Cancelled")
            return true;

        appointment.Status = "Cancelled";
        appointment.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.Appointments.UpdateAsync(appointment);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}