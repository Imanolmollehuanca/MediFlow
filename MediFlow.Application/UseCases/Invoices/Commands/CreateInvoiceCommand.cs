
using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Invoices.Commands;

public record CreateInvoiceCommand(CreateInvoiceDto Dto)
    : IRequest<InvoiceDto>;

public class CreateInvoiceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (dto.PaymentId <= 0)
            throw new ArgumentException("El pago es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.InvoiceNumber))
            throw new ArgumentException("El número de factura es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.InvoiceType))
            throw new ArgumentException("El tipo de comprobante es obligatorio.");

        var payment = await unitOfWork.Payments
            .GetByIdAsync(dto.PaymentId);

        if (payment is null)
            throw new ArgumentException("El pago no existe.");

        if (payment.Status != "Completed")
            throw new InvalidOperationException(
                "Solo se puede emitir un comprobante para un pago completado.");

        var invoices = await unitOfWork.Invoices.GetAllAsync();

        if (invoices.Any(i => i.PaymentId == dto.PaymentId))
            throw new InvalidOperationException(
                "Este pago ya tiene un comprobante registrado.");

        if (invoices.Any(i => i.InvoiceNumber == dto.InvoiceNumber.Trim()))
            throw new InvalidOperationException(
                "El número de comprobante ya está registrado.");

        var invoice = new Invoice
        {
            PaymentId = dto.PaymentId,
            InvoiceNumber = dto.InvoiceNumber.Trim(),
            InvoiceType = dto.InvoiceType.Trim(),
            IssuedAt = DateTime.UtcNow,
            TotalAmount = payment.Amount,
            CustomerName = dto.CustomerName?.Trim(),
            CustomerDocument = dto.CustomerDocument?.Trim()
        };

        await unitOfWork.Invoices.AddAsync(invoice);
        await unitOfWork.SaveChangesAsync();

        return InvoiceDto.FromEntity(invoice);
    }
}
