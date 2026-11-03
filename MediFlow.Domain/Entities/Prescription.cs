namespace MediFlow.Domain.Entities;

public class Prescription
{
    public int PrescriptionId { get; set; }

    public int DiagnosisId { get; set; }

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public string? GeneralInstructions { get; set; }

    public bool IsActive { get; set; } = true;

    public Diagnosis Diagnosis { get; set; } = null!;

    public ICollection<PrescriptionDetail> Details { get; set; }
        = new List<PrescriptionDetail>();
}