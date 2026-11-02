using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Application.UseCases.Appointments;

public class AppointmentUseCase(MediFlowContext context)
{
    private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Pending", "Confirmed", "Cancelled", "Completed"
    };

    public async Task<IReadOnlyList<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.Appointments.AsNoTracking().Include(a => a.Patient).Include(a => a.Doctor)
            .OrderBy(a => a.StartDateTime).SelectToDto().ToListAsync(cancellationToken);

    public async Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var appointment = await context.Appointments.AsNoTracking().Include(a => a.Patient).Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentId == id, cancellationToken);
        return appointment is null ? null : ToDto(appointment);
    }

    public async Task<AppointmentDto> CreateAsync(AppointmentRequestDto request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, null, cancellationToken);
        var appointment = new Appointment
        {
            PatientId = request.PatientId, DoctorId = request.DoctorId,
            StartDateTime = request.StartDateTime.ToUniversalTime(), EndDateTime = request.EndDateTime.ToUniversalTime(),
            Status = NormalizeStatus(request.Status), Reason = request.Reason?.Trim(), CreatedAt = DateTime.UtcNow
        };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync(cancellationToken);
        await context.Entry(appointment).Reference(a => a.Patient).LoadAsync(cancellationToken);
        await context.Entry(appointment).Reference(a => a.Doctor).LoadAsync(cancellationToken);
        return ToDto(appointment);
    }

    public async Task<AppointmentDto?> UpdateAsync(int id, AppointmentRequestDto request, CancellationToken cancellationToken)
    {
        var appointment = await context.Appointments.Include(a => a.Patient).Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.AppointmentId == id, cancellationToken);
        if (appointment is null) return null;
        await ValidateAsync(request, id, cancellationToken);
        appointment.PatientId = request.PatientId; appointment.DoctorId = request.DoctorId;
        appointment.StartDateTime = request.StartDateTime.ToUniversalTime(); appointment.EndDateTime = request.EndDateTime.ToUniversalTime();
        appointment.Status = NormalizeStatus(request.Status); appointment.Reason = request.Reason?.Trim(); appointment.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        await context.Entry(appointment).Reference(a => a.Patient).LoadAsync(cancellationToken);
        await context.Entry(appointment).Reference(a => a.Doctor).LoadAsync(cancellationToken);
        return ToDto(appointment);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var appointment = await context.Appointments.FindAsync([id], cancellationToken);
        if (appointment is null) return false;
        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task ValidateAsync(AppointmentRequestDto request, int? excludedId, CancellationToken cancellationToken)
    {
        if (request.StartDateTime.Kind == DateTimeKind.Unspecified || request.EndDateTime.Kind == DateTimeKind.Unspecified)
            throw new BusinessRuleException("Las fechas deben incluir zona horaria (por ejemplo, 2026-11-03T14:00:00Z).");
        var start = request.StartDateTime.ToUniversalTime(); var end = request.EndDateTime.ToUniversalTime();
        if (end <= start) throw new BusinessRuleException("La hora de fin debe ser posterior a la hora de inicio.");
        if (!ValidStatuses.Contains(request.Status?.Trim() ?? string.Empty))
            throw new BusinessRuleException("El estado debe ser Pending, Confirmed, Cancelled o Completed.");
        if (!await context.Patients.AnyAsync(p => p.PatientId == request.PatientId && p.IsActive, cancellationToken))
            throw new BusinessRuleException("El paciente no existe o está inactivo.");
        if (!await context.Doctors.AnyAsync(d => d.DoctorId == request.DoctorId && d.IsActive, cancellationToken))
            throw new BusinessRuleException("El médico no existe o está inactivo.");

        var weekday = (short)start.DayOfWeek;
        var startTime = TimeOnly.FromDateTime(start); var endTime = TimeOnly.FromDateTime(end);
        var schedules = await context.Schedules.Where(s => s.DoctorId == request.DoctorId && s.DayOfWeek == weekday && s.IsActive)
            .ToListAsync(cancellationToken);
        var fitsSchedule = start.Date == end.Date && schedules.Any(s =>
            startTime >= s.StartTime && endTime <= s.EndTime &&
            (end - start).TotalMinutes == s.SlotDurationMinutes &&
            (startTime - s.StartTime).TotalMinutes % s.SlotDurationMinutes == 0);
        if (!fitsSchedule) throw new BusinessRuleException("La cita no coincide con un horario disponible o con la duración del turno del médico.");

        var conflicts = await context.Appointments.AnyAsync(a => a.DoctorId == request.DoctorId &&
            (!excludedId.HasValue || a.AppointmentId != excludedId.Value) &&
            a.Status != "Cancelled" && a.StartDateTime < end && a.EndDateTime > start, cancellationToken);
        if (conflicts) throw new BusinessRuleException("El médico ya tiene una cita en ese intervalo.");
    }

    private static AppointmentDto ToDto(Appointment a) => new()
    {
        AppointmentId = a.AppointmentId, PatientId = a.PatientId, PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
        DoctorId = a.DoctorId, DoctorName = $"{a.Doctor.FirstName} {a.Doctor.LastName}", StartDateTime = a.StartDateTime,
        EndDateTime = a.EndDateTime, Status = a.Status, Reason = a.Reason, CreatedAt = a.CreatedAt, UpdatedAt = a.UpdatedAt
    };

    private static string NormalizeStatus(string status) => status.Trim().ToLowerInvariant() switch
    {
        "pending" => "Pending", "confirmed" => "Confirmed", "cancelled" => "Cancelled", "completed" => "Completed",
        _ => status.Trim()
    };
}

internal static class AppointmentQueryExtensions
{
    public static IQueryable<AppointmentDto> SelectToDto(this IQueryable<Appointment> query) => query.Select(a => new AppointmentDto
    {
        AppointmentId = a.AppointmentId, PatientId = a.PatientId, PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
        DoctorId = a.DoctorId, DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName, StartDateTime = a.StartDateTime,
        EndDateTime = a.EndDateTime, Status = a.Status, Reason = a.Reason, CreatedAt = a.CreatedAt, UpdatedAt = a.UpdatedAt
    });
}
