using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Commands;

public record CreateScheduleCommand(CreateScheduleDto Dto) : IRequest<ScheduleDto>;

internal sealed class CreateScheduleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateScheduleCommand, ScheduleDto>
{
    public async Task<ScheduleDto> Handle(
        CreateScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        await ScheduleValidation.ValidateAsync(
            unitOfWork, dto.DoctorId, dto.DayOfWeek, dto.StartTime, dto.EndTime,
            dto.SlotDurationMinutes, isActive: true, excludedScheduleId: null, cancellationToken);

        var schedule = dto.ToEntity();
        await unitOfWork.Schedules.AddAsync(schedule);
        await unitOfWork.SaveChangesAsync();

        return ScheduleDto.FromEntity(schedule);
    }
}
