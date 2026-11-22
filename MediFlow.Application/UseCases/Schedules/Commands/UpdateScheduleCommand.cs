using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Commands;

public record UpdateScheduleCommand(int ScheduleId, UpdateScheduleDto Dto) : IRequest<bool>;

internal sealed class UpdateScheduleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateScheduleCommand, bool>
{
    public async Task<bool> Handle(
        UpdateScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var schedule = await unitOfWork.Schedules.GetByIdAsync(request.ScheduleId);
        if (schedule is null)
            return false;

        var dto = request.Dto;
        await ScheduleValidation.ValidateAsync(
            unitOfWork, dto.DoctorId, dto.DayOfWeek, dto.StartTime, dto.EndTime,
            dto.SlotDurationMinutes, dto.IsActive, schedule.ScheduleId, cancellationToken);

        dto.ApplyTo(schedule);
        await unitOfWork.Schedules.UpdateAsync(schedule);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}
