using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Schedules.Commands;

internal static class ScheduleValidation
{
    public static async Task ValidateAsync(
        IUnitOfWork unitOfWork,
        int doctorId,
        short dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int slotDurationMinutes,
        bool isActive,
        int? excludedScheduleId,
        CancellationToken cancellationToken)
    {
        if (doctorId <= 0)
            throw new ArgumentException("El ID del médico debe ser positivo.");

        if (dayOfWeek is < 0 or > 6)
            throw new ArgumentException("El día de la semana debe estar entre 0 y 6.");

        if (startTime >= endTime)
            throw new ArgumentException("La hora de inicio debe ser menor que la hora de fin.");

        if (slotDurationMinutes <= 0)
            throw new ArgumentException("La duración de cada cita debe ser mayor que cero.");

        if (TimeSpan.FromMinutes(slotDurationMinutes) > endTime - startTime)
            throw new ArgumentException("La duración de la cita no puede superar el horario de atención.");

        var doctor = await unitOfWork.Doctors.GetByIdAsync(doctorId)
            ?? throw new KeyNotFoundException("El médico no existe.");

        if (!isActive)
            return;

        if (!doctor.IsActive)
            throw new InvalidOperationException("El horario activo requiere un médico activo.");

        if (await unitOfWork.Schedules.HasOverlappingScheduleAsync(
                doctorId, dayOfWeek, startTime, endTime, excludedScheduleId, cancellationToken))
        {
            throw new InvalidOperationException("El médico ya tiene un horario que se solapa en ese día.");
        }

        if (await unitOfWork.Schedules.HasRoomConflictAsync(
                doctorId, dayOfWeek, startTime, endTime, excludedScheduleId, cancellationToken))
        {
            throw new InvalidOperationException("Una sala asignada al médico ya está ocupada por otro médico en ese horario.");
        }
    }
}
