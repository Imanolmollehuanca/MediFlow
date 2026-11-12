using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class DoctorRoom
{
    public int DoctorRoomId { get; set; }

    public int DoctorId { get; set; }

    public int RoomId { get; set; }

    public DateTime AssignedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
