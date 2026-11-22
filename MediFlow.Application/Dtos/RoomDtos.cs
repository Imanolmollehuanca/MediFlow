using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record RoomDto(
    int RoomId,
    string RoomNumber,
    string? Description,
    string? Floor,
    bool IsActive)
{
    public static RoomDto FromEntity(Room room) => new(
        room.RoomId,
        room.RoomNumber,
        room.Description,
        room.Floor,
        room.IsActive);
}

public record CreateRoomDto(
    string RoomNumber,
    string? Description,
    string? Floor)
{
    public Room ToEntity() => new()
    {
        RoomNumber = RoomNumber.Trim(),
        Description = Description,
        Floor = Floor,
        IsActive = true
    };
}

public record UpdateRoomDto(
    string RoomNumber,
    string? Description,
    string? Floor)
{
    public void ApplyTo(Room room)
    {
        room.RoomNumber = RoomNumber.Trim();
        room.Description = Description;
        room.Floor = Floor;
    }
}
