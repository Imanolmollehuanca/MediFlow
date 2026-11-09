namespace MediFlow.Domain.Entities;

public class Payment
{
    public int PaymentId { get; set; }

    public int AppointmentId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public string? TransactionReference { get; set; }

    public Appointment Appointment { get; set; } = null!;

    public Invoice? Invoice { get; set; }
}