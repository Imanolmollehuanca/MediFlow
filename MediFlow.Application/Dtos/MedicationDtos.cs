using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record MedicationDto(
    int MedicationId,
    string Name, string? Description, string? Presentation,
    string? Concentration, bool IsActive, DateTime CreatedAt
)
{
    public MedicationDto(Medication medication) : this(
        medication.MedicationId, medication.Name, medication.Description,
        medication.Presentation, medication.Concentration, medication.IsActive, medication.CreatedAt
    )
    {
    }
}

public record CreateMedicationDto(
    string Name, string? Description,
    string? Presentation, string? Concentration
)
{
    public Medication ToEntity()
    {
        return new Medication
        {
            Name = Name.Trim(),
            Description = Description,
            Presentation = Presentation,
            Concentration = Concentration,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}

public record UpdateMedicationDto(
    string Name, string? Description, string? Presentation,
    string? Concentration, bool IsActive
)
{
    public void ApplyTo(Medication medication)
    {
        medication.Name = Name.Trim();
        medication.Description = Description;
        medication.Presentation = Presentation;
        medication.Concentration = Concentration;
        medication.IsActive = IsActive;
    }
}