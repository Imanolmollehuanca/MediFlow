namespace MediFlow.Application.Dtos;

public record DoctorDto(int DoctorId, int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email, bool IsActive,
    DateTime CreatedAt );

public record CreateDoctorDto(int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email );
public record UpdateDoctorDto(int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email );