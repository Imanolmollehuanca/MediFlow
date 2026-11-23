using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record ScheduleDto(
    int ScheduleId,
    int DoctorId,
    short DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes,
    bool IsActive)
{
    public static ScheduleDto FromEntity(Schedule schedule) => new(
        schedule.ScheduleId,
        schedule.DoctorId,
        schedule.DayOfWeek,
        schedule.StartTime,
        schedule.EndTime,
        schedule.SlotDurationMinutes,
        schedule.IsActive);
}

public record ScheduleAvailabilityDto(
    int ScheduleId,
    int DoctorId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    int SlotDurationMinutes,
    IReadOnlyList<int> RoomIds)
{
    public static ScheduleAvailabilityDto FromEntity(
        Schedule schedule, DateTime startDateTime, IReadOnlyList<int> roomIds) => new(
            schedule.ScheduleId,
            schedule.DoctorId,
            startDateTime,
            startDateTime.AddMinutes(schedule.SlotDurationMinutes),
            schedule.SlotDurationMinutes,
            roomIds);
}

public record CreateScheduleDto(
    int DoctorId,
    short DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes)
{
    public Schedule ToEntity() => new()
    {
        DoctorId = DoctorId,
        DayOfWeek = DayOfWeek,
        StartTime = StartTime,
        EndTime = EndTime,
        SlotDurationMinutes = SlotDurationMinutes,
        IsActive = true
    };
}

public record UpdateScheduleDto(
    int DoctorId,
    short DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes,
    bool IsActive)
{
    public void ApplyTo(Schedule schedule)
    {
        schedule.DoctorId = DoctorId;
        schedule.DayOfWeek = DayOfWeek;
        schedule.StartTime = StartTime;
        schedule.EndTime = EndTime;
        schedule.SlotDurationMinutes = SlotDurationMinutes;
        schedule.IsActive = IsActive;
    }
}
