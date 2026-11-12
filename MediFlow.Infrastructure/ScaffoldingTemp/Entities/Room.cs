using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string? Description { get; set; }

    public string? Floor { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<DoctorRoom> DoctorRooms { get; set; } = new List<DoctorRoom>();
}
