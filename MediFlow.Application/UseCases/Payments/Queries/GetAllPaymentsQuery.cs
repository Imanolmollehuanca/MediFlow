using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Payments.Queries;

public record GetAllPaymentsQuery() : IRequest<List<PaymentDto>>;

public class GetAllPaymentsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllPaymentsQuery, List<PaymentDto>>
{
    public async Task<List<PaymentDto>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var payments = await unitOfWork.Payments.GetAllAsync();

        return payments
            .Select(PaymentDto.FromEntity)
            .ToList();
    }
}