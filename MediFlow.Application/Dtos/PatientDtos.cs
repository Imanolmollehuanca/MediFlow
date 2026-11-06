using MediFlow.Domain.Entities;
namespace MediFlow.Application.Dtos;

public record PatientDto(int PatientId, string DocumentType, string DocumentNumber, string FirstName, string LastName,
    DateOnly BirthDate, string Gender, string Phone, string? Email, string? Address, bool IsActive, DateTime CreatedAt )
{
    public PatientDto(Patient patient) : this(patient.PatientId, patient.DocumentType, patient.DocumentNumber, patient.FirstName, patient.LastName, patient.BirthDate, patient.Gender,
        patient.Phone, patient.Email, patient.Address, patient.IsActive, patient.CreatedAt)
    {
    }
}

public record CreatePatientDto(string DocumentType, string DocumentNumber, string FirstName, string LastName,
    DateOnly BirthDate, string Gender, string Phone, string? Email, string? Address
);

public record UpdatePatientDto(string DocumentType, string DocumentNumber, string FirstName, string LastName,
    DateOnly BirthDate, string Gender, string Phone, string? Email, string? Address
);