using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class Medication
{
    public int MedicationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Presentation { get; set; }

    public string? Concentration { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}
