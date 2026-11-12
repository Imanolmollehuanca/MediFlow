using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int DiagnosisId { get; set; }

    public DateTime IssuedAt { get; set; }

    public string? GeneralInstructions { get; set; }

    public bool IsActive { get; set; }

    public virtual Diagnosis Diagnosis { get; set; } = null!;

    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}
