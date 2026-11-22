using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record PrescriptionDto(
    int PrescriptionId, int DiagnosisId,
    DateTime IssuedAt, string? GeneralInstructions, bool IsActive
)
{
    public PrescriptionDto(Prescription prescription) : this(
        prescription.PrescriptionId, prescription.DiagnosisId, prescription.IssuedAt,
        prescription.GeneralInstructions, prescription.IsActive
    )
    {
    }
}

public record CreatePrescriptionDto(
    int DiagnosisId,
    string? GeneralInstructions
)
{
    public Prescription ToEntity()
    {
        return new Prescription
        {
            DiagnosisId = DiagnosisId,
            GeneralInstructions = GeneralInstructions,
            IssuedAt = DateTime.UtcNow,
            IsActive = true
        };
    }
}

public record UpdatePrescriptionDto(
    string? GeneralInstructions, bool IsActive
)
{
    public void ApplyTo(Prescription prescription)
    {
        prescription.GeneralInstructions = GeneralInstructions;
        prescription.IsActive = IsActive;
    }
}