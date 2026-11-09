namespace MediFlow.Domain.Entities;

public class Notification
{
    public int NotificationId { get; set; }

    public int PatientId { get; set; }

    public int? AppointmentId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Type { get; set; } = "Reminder";

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAt { get; set; }

    public Patient Patient { get; set; } = null!;

    public Appointment? Appointment { get; set; }
}