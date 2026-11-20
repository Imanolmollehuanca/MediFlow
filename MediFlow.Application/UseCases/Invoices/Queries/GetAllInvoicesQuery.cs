using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Invoices.Queries;

public record GetAllInvoicesQuery() : IRequest<List<InvoiceDto>>;

public class GetAllInvoicesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllInvoicesQuery, List<InvoiceDto>>
{
    public async Task<List<InvoiceDto>> Handle(
        GetAllInvoicesQuery request,
        CancellationToken cancellationToken)
    {
        var invoices = await unitOfWork.Invoices.GetAllAsync();

        return invoices
            .Select(InvoiceDto.FromEntity)
            .ToList();
    }
}