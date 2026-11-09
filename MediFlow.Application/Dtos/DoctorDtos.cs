using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record DoctorDto(int DoctorId, int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email, bool IsActive, DateTime CreatedAt)
{
    public DoctorDto(Doctor doctor) : this(doctor.DoctorId, doctor.SpecialtyId, doctor.FirstName, doctor.LastName, doctor.LicenseNumber, doctor.Phone, doctor.Email, doctor.IsActive, doctor.CreatedAt)
    {
    }
}

public record CreateDoctorDto(int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email)
{
    public Doctor ToEntity() => new()
    { SpecialtyId = SpecialtyId, FirstName = FirstName.Trim(), LastName = LastName.Trim(), LicenseNumber = LicenseNumber.Trim(),
        Phone = Phone.Trim(), Email = Email.Trim(), IsActive = true, CreatedAt = DateTime.UtcNow };
}

public record UpdateDoctorDto(int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email);