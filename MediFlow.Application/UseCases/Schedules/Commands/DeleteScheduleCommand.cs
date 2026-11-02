using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Commands;

public record DeleteScheduleCommand(int ScheduleId) : IRequest<bool>;

internal sealed class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, bool>
{
    private readonly IGenericRepository<Schedule> _repository;

    public DeleteScheduleCommandHandler(IGenericRepository<Schedule> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _repository.GetByIdAsync(request.ScheduleId);

        if (schedule is null || !schedule.IsActive)
            return false;

        schedule.IsActive = false;
        await _repository.UpdateAsync(schedule);
        return true;
    }
}