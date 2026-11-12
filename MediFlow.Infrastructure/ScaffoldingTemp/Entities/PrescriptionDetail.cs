using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class PrescriptionDetail
{
    public int PrescriptionDetailId { get; set; }

    public int PrescriptionId { get; set; }

    public int MedicationId { get; set; }

    public string Dosage { get; set; } = null!;

    public string Frequency { get; set; } = null!;

    public int DurationDays { get; set; }

    public int Quantity { get; set; }

    public string? Instructions { get; set; }

    public virtual Medication Medication { get; set; } = null!;

    public virtual Prescription Prescription { get; set; } = null!;
}
