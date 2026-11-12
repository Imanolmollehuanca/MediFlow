using System;
using System.Collections.Generic;

namespace MediFlow.Infrastructure.ScaffoldingTemp.Entities;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int PaymentId { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public string InvoiceType { get; set; } = null!;

    public DateTime IssuedAt { get; set; }

    public decimal TotalAmount { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerDocument { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
