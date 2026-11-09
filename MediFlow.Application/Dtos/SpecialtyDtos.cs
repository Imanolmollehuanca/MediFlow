using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record SpecialtyDto(int SpecialtyId, string Name, string? Description, bool IsActive, DateTime CreatedAt )
{
    public SpecialtyDto(Specialty specialty) : this(specialty.SpecialtyId, specialty.Name, specialty.Description, specialty.IsActive, specialty.CreatedAt)
    {
    }
}

public record CreateSpecialtyDto(string Name, string? Description );

public record UpdateSpecialtyDto(string Name, string? Description );