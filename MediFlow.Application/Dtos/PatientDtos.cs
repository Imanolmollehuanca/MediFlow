namespace MediFlow.Application.Dtos;

public record PatientDto(int PatientId, string DocumentType, string DocumentNumber, string FirstName, string LastName,
    DateOnly BirthDate, string Gender, string Phone, string? Email, string? Address, bool IsActive,
    DateTime CreatedAt
);

public record CreatePatientDto(string DocumentType, string DocumentNumber, string FirstName, string LastName,
    DateOnly BirthDate, string Gender, string Phone, string? Email, string? Address
);

public record UpdatePatientDto(string DocumentType, string DocumentNumber, string FirstName, string LastName,
    DateOnly BirthDate, string Gender, string Phone, string? Email, string? Address
);