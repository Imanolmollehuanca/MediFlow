using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Payments.Queries;

public record GetPaymentByIdQuery(int PaymentId)
    : IRequest<PaymentDto?>;

public class GetPaymentByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPaymentByIdQuery, PaymentDto?>
{
    public async Task<PaymentDto?> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var payment = await unitOfWork.Payments
            .GetByIdAsync(request.PaymentId);

        return payment is null
            ? null
            : PaymentDto.FromEntity(payment);
    }
}