using System.ComponentModel.DataAnnotations;

namespace MediFlow.Application.Dtos;

public class ScheduleRequestDto
{
    [Range(1, int.MaxValue)] public int DoctorId { get; init; }
    [Range(0, 6)] public short DayOfWeek { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    [Range(1, 1440)] public int SlotDurationMinutes { get; init; }
    public bool IsActive { get; init; } = true;
}

public class ScheduleDto
{
    public int ScheduleId { get; init; }
    public int DoctorId { get; init; }
    public string DoctorName { get; init; } = string.Empty;
    public short DayOfWeek { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public int SlotDurationMinutes { get; init; }
    public bool IsActive { get; init; }
}
