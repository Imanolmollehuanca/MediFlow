namespace MediFlow.Application.Dtos;

public record ScheduleDto(int ScheduleId, int DoctorId, short DayOfWeek,
    TimeOnly StartTime, TimeOnly EndTime, int SlotDurationMinutes, bool IsActive );

public record CreateScheduleDto(int DoctorId, short DayOfWeek,
    TimeOnly StartTime, TimeOnly EndTime, int SlotDurationMinutes );

public record UpdateScheduleDto(int DoctorId, short DayOfWeek,
    TimeOnly StartTime, TimeOnly EndTime, int SlotDurationMinutes );