namespace MediFlow.Domain.Entities;

public class PrescriptionDetail
{
    public int PrescriptionDetailId { get; set; }

    public int PrescriptionId { get; set; }

    public int MedicationId { get; set; }

    public string Dosage { get; set; } = string.Empty;

    public string Frequency { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    public int Quantity { get; set; }

    public string? Instructions { get; set; }

    public Prescription Prescription { get; set; } = null!;

    public Medication Medication { get; set; } = null!;
}