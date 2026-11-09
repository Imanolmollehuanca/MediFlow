namespace MediFlow.Domain.Entities;

public class MedicalRecord
{
    public int MedicalRecordId { get; set; }

    public int PatientId { get; set; }

    public string? BloodType { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalHistory { get; set; }

    public string? Observations { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public Patient Patient { get; set; } = null!;

    public ICollection<Diagnosis> Diagnoses { get; set; }
        = new List<Diagnosis>();
}