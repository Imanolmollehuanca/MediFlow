using MediFlow.Domain.Entities;
namespace MediFlow.Application.Dtos;

public record MedicalRecordDto(int MedicalRecordId, int PatientId, string? BloodType, string? Allergies, string? MedicalHistory,
    string? Observations, DateTime CreatedAt, DateTime? UpdatedAt)
{
    public MedicalRecordDto(MedicalRecord record) : this(
        record.MedicalRecordId, record.PatientId, record.BloodType, record.Allergies,
        record.MedicalHistory, record.Observations, record.CreatedAt, record.UpdatedAt)
    {
    }
}

public record CreateMedicalRecordDto(int PatientId, string? BloodType,
    string? Allergies, string? MedicalHistory, string? Observations)
{
    public MedicalRecord ToEntity() => new()
    {
        PatientId = PatientId, BloodType = BloodType, Allergies = Allergies,
        MedicalHistory = MedicalHistory, Observations = Observations, CreatedAt = DateTime.UtcNow
    };
}

public record UpdateMedicalRecordDto(string? BloodType, string? Allergies, string? MedicalHistory, string? Observations)
{
    public void ApplyTo(MedicalRecord record)
    {
        record.BloodType = BloodType; record.Allergies = Allergies;
        record.MedicalHistory = MedicalHistory; record.Observations = Observations; record.UpdatedAt = DateTime.UtcNow;
    }
}