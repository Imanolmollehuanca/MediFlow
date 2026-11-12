using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class Diagnosis
{
    public int DiagnosisId { get; set; }

    public int MedicalRecordId { get; set; }

    public int AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public string Description { get; set; } = null!;

    public string? Icdcode { get; set; }

    public string? Observations { get; set; }

    public DateTime DiagnosisDate { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual MedicalRecord MedicalRecord { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
