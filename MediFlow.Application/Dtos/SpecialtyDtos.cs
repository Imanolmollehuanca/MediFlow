namespace MediFlow.Application.Dtos;

public record SpecialtyDto(int SpecialtyId, string Name, string? Description, bool IsActive,
    DateTime CreatedAt );
public record CreateSpecialtyDto(string Name, string? Description );
public record UpdateSpecialtyDto(string Name, string? Description );