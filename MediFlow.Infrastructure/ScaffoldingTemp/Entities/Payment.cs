using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int AppointmentId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public string? TransactionReference { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Invoice? Invoice { get; set; }
}
