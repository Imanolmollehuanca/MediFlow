using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Invoices.Queries;

public record GetInvoiceByIdQuery(int InvoiceId) : IRequest<InvoiceDto?>;

public class GetInvoiceByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto?>
{
    public async Task<InvoiceDto?> Handle(
        GetInvoiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var invoice = await unitOfWork.Invoices
            .GetByIdAsync(request.InvoiceId);

        return invoice is null
            ? null
            : InvoiceDto.FromEntity(invoice);
    }
}