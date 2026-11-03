namespace MediFlow.Domain.Entities;

public class Diagnosis
{
    public int DiagnosisId { get; set; }

    public int MedicalRecordId { get; set; }

    public int AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? ICDCode { get; set; }

    public string? Observations { get; set; }

    public DateTime DiagnosisDate { get; set; } = DateTime.UtcNow;

    public MedicalRecord MedicalRecord { get; set; } = null!;

    public Appointment Appointment { get; set; } = null!;

    public Doctor Doctor { get; set; } = null!;

    public ICollection<Prescription> Prescriptions { get; set; }
        = new List<Prescription>();
}