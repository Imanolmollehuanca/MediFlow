using MediFlow.Domain.Entities;
namespace MediFlow.Application.Dtos;

public record DiagnosisDto(int DiagnosisId, int MedicalRecordId, int AppointmentId, int DoctorId,
    string Description, string? ICDCode, string? Observations, DateTime DiagnosisDate)
{
    public DiagnosisDto(Diagnosis diagnosis) : this(
        diagnosis.DiagnosisId, diagnosis.MedicalRecordId, diagnosis.AppointmentId, diagnosis.DoctorId,
        diagnosis.Description, diagnosis.ICDCode, diagnosis.Observations, diagnosis.DiagnosisDate)
    {
    }
}

public record CreateDiagnosisDto(
    int MedicalRecordId, int AppointmentId, int DoctorId,
    string Description, string? ICDCode, string? Observations)
{
    public Diagnosis ToEntity() => new()
    {
        MedicalRecordId = MedicalRecordId, AppointmentId = AppointmentId, DoctorId = DoctorId, Description = Description, ICDCode = ICDCode,
        Observations = Observations, DiagnosisDate = DateTime.UtcNow
    };
}

public record UpdateDiagnosisDto(
    string Description, string? ICDCode, string? Observations)
{
    public void ApplyTo(Diagnosis diagnosis)
    {
        diagnosis.Description = Description;
        diagnosis.ICDCode = ICDCode;
        diagnosis.Observations = Observations;
    }
}