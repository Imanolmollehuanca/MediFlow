using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases;
using MediFlow.Domain.Entities;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Application.UseCases.Schedules;

public class ScheduleUseCase(MediFlowContext context)
{
    public async Task<IReadOnlyList<ScheduleDto>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.Schedules.AsNoTracking().Include(s => s.Doctor).OrderBy(s => s.DoctorId).ThenBy(s => s.DayOfWeek).ThenBy(s => s.StartTime)
            .SelectToDto().ToListAsync(cancellationToken);

    public async Task<ScheduleDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var schedule = await context.Schedules.AsNoTracking().Include(s => s.Doctor)
            .FirstOrDefaultAsync(s => s.ScheduleId == id, cancellationToken);
        return schedule is null ? null : ToDto(schedule);
    }

    public async Task<ScheduleDto> CreateAsync(ScheduleRequestDto request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, null, cancellationToken);
        var schedule = new Schedule { DoctorId = request.DoctorId, DayOfWeek = request.DayOfWeek, StartTime = request.StartTime,
            EndTime = request.EndTime, SlotDurationMinutes = request.SlotDurationMinutes, IsActive = request.IsActive };
        context.Schedules.Add(schedule);
        await context.SaveChangesAsync(cancellationToken);
        await context.Entry(schedule).Reference(s => s.Doctor).LoadAsync(cancellationToken);
        return ToDto(schedule);
    }

    public async Task<ScheduleDto?> UpdateAsync(int id, ScheduleRequestDto request, CancellationToken cancellationToken)
    {
        var schedule = await context.Schedules.Include(s => s.Doctor).FirstOrDefaultAsync(s => s.ScheduleId == id, cancellationToken);
        if (schedule is null) return null;
        await ValidateAsync(request, id, cancellationToken);
        schedule.DoctorId = request.DoctorId; schedule.DayOfWeek = request.DayOfWeek; schedule.StartTime = request.StartTime;
        schedule.EndTime = request.EndTime; schedule.SlotDurationMinutes = request.SlotDurationMinutes; schedule.IsActive = request.IsActive;
        await context.SaveChangesAsync(cancellationToken);
        await context.Entry(schedule).Reference(s => s.Doctor).LoadAsync(cancellationToken);
        return ToDto(schedule);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var schedule = await context.Schedules.FindAsync([id], cancellationToken);
        if (schedule is null) return false;
        context.Schedules.Remove(schedule);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task ValidateAsync(ScheduleRequestDto request, int? excludedId, CancellationToken cancellationToken)
    {
        if (request.EndTime <= request.StartTime) throw new BusinessRuleException("La hora de fin debe ser posterior a la hora de inicio.");
        var duration = request.EndTime - request.StartTime;
        if (duration.TotalMinutes % request.SlotDurationMinutes != 0)
            throw new BusinessRuleException("La duración del horario debe ser múltiplo de la duración del turno.");
        if (!await context.Doctors.AnyAsync(d => d.DoctorId == request.DoctorId && d.IsActive, cancellationToken))
            throw new BusinessRuleException("El médico no existe o está inactivo.");
        var overlaps = await context.Schedules.AnyAsync(s => s.DoctorId == request.DoctorId && s.DayOfWeek == request.DayOfWeek &&
            (!excludedId.HasValue || s.ScheduleId != excludedId.Value) && s.StartTime < request.EndTime && s.EndTime > request.StartTime, cancellationToken);
        if (overlaps) throw new BusinessRuleException("El médico ya tiene un horario que se superpone con este intervalo.");
    }

    private static ScheduleDto ToDto(Schedule s) => new()
    {
        ScheduleId = s.ScheduleId, DoctorId = s.DoctorId, DoctorName = $"{s.Doctor.FirstName} {s.Doctor.LastName}",
        DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime, SlotDurationMinutes = s.SlotDurationMinutes, IsActive = s.IsActive
    };
}

internal static class ScheduleQueryExtensions
{
    public static IQueryable<ScheduleDto> SelectToDto(this IQueryable<Schedule> query) => query.Select(s => new ScheduleDto
    {
        ScheduleId = s.ScheduleId, DoctorId = s.DoctorId, DoctorName = s.Doctor.FirstName + " " + s.Doctor.LastName,
        DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime, SlotDurationMinutes = s.SlotDurationMinutes, IsActive = s.IsActive
    });
}
