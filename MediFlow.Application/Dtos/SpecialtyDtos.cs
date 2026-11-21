using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record SpecialtyDto(
    int SpecialtyId,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAt)
{
    public static SpecialtyDto FromEntity(Specialty specialty) => new(
        specialty.SpecialtyId,
        specialty.Name,
        specialty.Description,
        specialty.IsActive,
        specialty.CreatedAt);
}

public record CreateSpecialtyDto(
    string Name,
    string? Description)
{
    public Specialty ToEntity() => new()
    {
        Name = Name.Trim(),
        Description = Description,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };
}

public record UpdateSpecialtyDto(
    string Name,
    string? Description)
{
    public void ApplyTo(Specialty specialty)
    {
        specialty.Name = Name.Trim();
        specialty.Description = Description;
    }
}
