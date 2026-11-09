namespace MediFlow.Domain.Entities;

public class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Floor { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<DoctorRoom> DoctorRooms { get; set; }
        = new List<DoctorRoom>();
}