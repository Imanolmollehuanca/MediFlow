namespace MediFlow.Domain.Entities;

public class Medication
{
    public int MedicationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Presentation { get; set; }

    public string? Concentration { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
        = new List<PrescriptionDetail>();
}