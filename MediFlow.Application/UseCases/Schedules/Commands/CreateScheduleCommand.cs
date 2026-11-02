using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Commands;

public record CreateScheduleCommand(int DoctorId, short DayOfWeek,
    TimeOnly StartTime, TimeOnly EndTime, int SlotDurationMinutes ) : IRequest<int>;

internal sealed class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, int>
{
    private readonly IGenericRepository<Schedule> _schedules;
    private readonly IGenericRepository<Doctor> _doctors;

    public CreateScheduleCommandHandler(IGenericRepository<Schedule> schedules, IGenericRepository<Doctor> doctors)
    {
        _schedules = schedules;
        _doctors = doctors;
    }

    public async Task<int> Handle(
        CreateScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await _doctors.GetByIdAsync(request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            throw new ArgumentException("El médico no existe o está inactivo.");

        if (request.DayOfWeek < 0 || request.DayOfWeek > 6)
            throw new ArgumentException("El día debe estar entre 0 y 6.");

        if (request.StartTime >= request.EndTime)
            throw new ArgumentException("La hora inicial debe ser menor que la final.");

        if (request.SlotDurationMinutes <= 0)
            throw new ArgumentException("La duración de cada turno debe ser positiva.");

        var schedule = new Schedule
        {
            DoctorId = request.DoctorId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            SlotDurationMinutes = request.SlotDurationMinutes,
            IsActive = true
        };
        await _schedules.AddAsync(schedule);
        return schedule.ScheduleId;
    }
}