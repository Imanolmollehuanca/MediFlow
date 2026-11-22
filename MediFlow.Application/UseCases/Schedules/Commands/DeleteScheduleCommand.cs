using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Commands;

public record DeleteScheduleCommand(int ScheduleId) : IRequest<bool>;

internal sealed class DeleteScheduleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteScheduleCommand, bool>
{
    public async Task<bool> Handle(
        DeleteScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var schedule = await unitOfWork.Schedules.GetByIdAsync(request.ScheduleId);
        if (schedule is null)
            return false;

        if (schedule.IsActive)
        {
            schedule.IsActive = false;
            await unitOfWork.Schedules.UpdateAsync(schedule);
            await unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
