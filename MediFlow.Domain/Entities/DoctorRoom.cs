namespace MediFlow.Domain.Entities;

public class DoctorRoom
{
    public int DoctorRoomId { get; set; }

    public int DoctorId { get; set; }

    public int RoomId { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public Doctor Doctor { get; set; } = null!;

    public Room Room { get; set; } = null!;
}