using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record PaymentDto(
    int PaymentId, int AppointmentId, decimal Amount,
    string PaymentMethod, string Status, DateTime PaymentDate, string? TransactionReference)
{
    public static PaymentDto FromEntity(Payment payment)
        => new(
            payment.PaymentId, payment.AppointmentId, payment.Amount, payment.PaymentMethod,
            payment.Status, payment.PaymentDate, payment.TransactionReference
        );
}