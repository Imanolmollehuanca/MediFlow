using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;


public record PrescriptionDetailDto(
    int PrescriptionDetailId, int PrescriptionId, int MedicationId,
    string Dosage, string Frequency, int DurationDays, int Quantity, string? Instructions
)
{
    public PrescriptionDetailDto(PrescriptionDetail detail) : this(
        detail.PrescriptionDetailId, detail.PrescriptionId, detail.MedicationId, detail.Dosage,
        detail.Frequency, detail.DurationDays, detail.Quantity, detail.Instructions
    )
    {
    }
}

public record CreatePrescriptionDetailDto(
    int PrescriptionId, int MedicationId, string Dosage,
    string Frequency, int DurationDays, int Quantity, string? Instructions
)
{
    public PrescriptionDetail ToEntity()
    {
        return new PrescriptionDetail
        {
            PrescriptionId = PrescriptionId,
            MedicationId = MedicationId,
            Dosage = Dosage.Trim(),
            Frequency = Frequency.Trim(),
            DurationDays = DurationDays,
            Quantity = Quantity,
            Instructions = Instructions
        };
    }
}

public record UpdatePrescriptionDetailDto(
    int MedicationId, string Dosage,
    string Frequency, int DurationDays,
    int Quantity, string? Instructions
)
{
    public void ApplyTo(PrescriptionDetail detail)
    {
        detail.MedicationId = MedicationId;
        detail.Dosage = Dosage.Trim();
        detail.Frequency = Frequency.Trim();
        detail.DurationDays = DurationDays;
        detail.Quantity = Quantity;
        detail.Instructions = Instructions;
    }
}