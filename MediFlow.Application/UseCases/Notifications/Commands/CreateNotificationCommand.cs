using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Notifications.Commands;

public record CreateNotificationCommand(CreateNotificationDto Dto)
    : IRequest<NotificationDto>;

public class CreateNotificationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    public async Task<NotificationDto> Handle(
        CreateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (dto.PatientId <= 0)
            throw new ArgumentException("El paciente es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("El título es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Message))
            throw new ArgumentException("El mensaje es obligatorio.");

        var patient = await unitOfWork.Patients.GetByIdAsync(dto.PatientId);

        if (patient is null)
            throw new ArgumentException("El paciente no existe.");

        if (dto.AppointmentId.HasValue)
        {
            var appointment = await unitOfWork.Appointments
                .GetByIdAsync(dto.AppointmentId.Value);

            if (appointment is null)
                throw new ArgumentException("La cita no existe.");
        }

        var notification = new Notification
        {
            PatientId = dto.PatientId,
            AppointmentId = dto.AppointmentId,
            Title = dto.Title.Trim(),
            Message = dto.Message.Trim(),
            Type = string.IsNullOrWhiteSpace(dto.Type)
                ? "Reminder"
                : dto.Type.Trim(),
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.Notifications.AddAsync(notification);
        await unitOfWork.SaveChangesAsync();

        return NotificationDto.FromEntity(notification);
    }
}