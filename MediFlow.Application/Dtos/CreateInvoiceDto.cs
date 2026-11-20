namespace MediFlow.Application.Dtos;

public record CreateInvoiceDto(
    int PaymentId, string InvoiceNumber, string InvoiceType, string? CustomerName, string? CustomerDocument);