namespace MediFlow.Domain.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }

    public int PaymentId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public string InvoiceType { get; set; } = string.Empty;

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerDocument { get; set; }

    public Payment Payment { get; set; } = null!;
}