using MediFlow.Domain.Entities;

namespace MediFlow.Application.Dtos;

public record InvoiceDto(
    int InvoiceId, int PaymentId, string InvoiceNumber,
    string InvoiceType, DateTime IssuedAt, decimal TotalAmount, string? CustomerName, string? CustomerDocument)
{
    public static InvoiceDto FromEntity(Invoice invoice)
        => new(
            invoice.InvoiceId, invoice.PaymentId, invoice.InvoiceNumber, invoice.InvoiceType,
            invoice.IssuedAt, invoice.TotalAmount, invoice.CustomerName, invoice.CustomerDocument
        );
}