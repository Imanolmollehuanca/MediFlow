namespace MediFlow.Domain.Entities;

public class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string Status { get; set; } = "Pending";

    public string? Reason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Relación con el paciente
    public Patient Patient { get; set; } = null!;

    // Relación con el médico
    public Doctor Doctor { get; set; } = null!;
}