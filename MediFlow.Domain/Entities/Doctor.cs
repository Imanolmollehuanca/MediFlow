namespace MediFlow.Domain.Entities;

public class Doctor
{
    public int DoctorId { get; set; }

    public int SpecialtyId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relación con especialidades
    public Specialty Specialty { get; set; } = null!;

    // Relación con horarios
    public ICollection<Schedule> Schedules { get; set; }
        = new List<Schedule>();

    // Relación con citas médicas
    public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();
}