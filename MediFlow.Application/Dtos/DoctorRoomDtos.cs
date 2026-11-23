using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record DoctorRoomDto(
    int DoctorRoomId,
    int DoctorId,
    int RoomId,
    DateTime AssignedAt,
    bool IsActive)
{
    public static DoctorRoomDto FromEntity(DoctorRoom assignment) => new(
        assignment.DoctorRoomId,
        assignment.DoctorId,
        assignment.RoomId,
        assignment.AssignedAt,
        assignment.IsActive);
}

public record CreateDoctorRoomDto(int DoctorId, int RoomId)
{
    public DoctorRoom ToEntity() => new()
    {
        DoctorId = DoctorId,
        RoomId = RoomId
    };
}

public record UpdateDoctorRoomDto(int DoctorId, int RoomId, bool IsActive)
{
    public void ApplyTo(DoctorRoom assignment)
    {
        assignment.DoctorId = DoctorId;
        assignment.RoomId = RoomId;
        assignment.IsActive = IsActive;
    }
}
