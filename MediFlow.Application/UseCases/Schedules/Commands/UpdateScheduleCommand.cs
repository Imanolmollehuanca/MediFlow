using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Commands;

public record UpdateScheduleCommand(int ScheduleId, int DoctorId, short DayOfWeek,
    TimeOnly StartTime, TimeOnly EndTime, int SlotDurationMinutes ) : IRequest<bool>;

internal sealed class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, bool>
{
    private readonly IGenericRepository<Schedule> _schedules;
    private readonly IGenericRepository<Doctor> _doctors;

    public UpdateScheduleCommandHandler(IGenericRepository<Schedule> schedules, IGenericRepository<Doctor> doctors)
    {
        _schedules = schedules;
        _doctors = doctors;
    }

    public async Task<bool> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _schedules.GetByIdAsync(request.ScheduleId);

        if (schedule is null || !schedule.IsActive)
            return false;

        var doctor = await _doctors.GetByIdAsync(request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            throw new ArgumentException("El médico no existe o está inactivo.");

        if (request.DayOfWeek < 0 || request.DayOfWeek > 6)
            throw new ArgumentException("El día debe estar entre 0 y 6.");

        if (request.StartTime >= request.EndTime)
            throw new ArgumentException("El horario es inválido.");

        if (request.SlotDurationMinutes <= 0)
            throw new ArgumentException("La duración debe ser positiva.");

        schedule.DoctorId = request.DoctorId;
        schedule.DayOfWeek = request.DayOfWeek;
        schedule.StartTime = request.StartTime;
        schedule.EndTime = request.EndTime;
        schedule.SlotDurationMinutes = request.SlotDurationMinutes;

        await _schedules.UpdateAsync(schedule);
        return true;
    }
}