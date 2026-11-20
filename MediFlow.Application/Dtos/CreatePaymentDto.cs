namespace MediFlow.Application.Dtos;

public record CreatePaymentDto(
    int AppointmentId, decimal Amount, string PaymentMethod, string? TransactionReference);