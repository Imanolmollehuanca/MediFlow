namespace MediFlow.Domain.Entities;

public class Specialty
{
    public int SpecialtyId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Doctor> Doctors { get; set; }
        = new List<Doctor>();
}