namespace MediFlow.Domain.Entities;

public class Schedule
{
    public int ScheduleId { get; set; }

    public int DoctorId { get; set; }

    public short DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int SlotDurationMinutes { get; set; }

    public bool IsActive { get; set; } = true;

    // Relación con el médico
    public Doctor Doctor { get; set; } = null!;
}