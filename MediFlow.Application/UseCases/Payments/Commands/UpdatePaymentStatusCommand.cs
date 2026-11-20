using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Payments.Commands;

public record UpdatePaymentStatusCommand(
    int PaymentId,
    UpdatePaymentStatusDto Dto)
    : IRequest<PaymentDto?>;

public class UpdatePaymentStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePaymentStatusCommand, PaymentDto?>
{
    public async Task<PaymentDto?> Handle(
        UpdatePaymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await unitOfWork.Payments
            .GetByIdAsync(request.PaymentId);

        if (payment is null)
            return null;

        var status = request.Dto.Status.Trim();

        if (status is not ("Pending" or "Completed" or "Failed"))
            throw new ArgumentException(
                "El estado debe ser Pending, Completed o Failed.");

        payment.Status = status;

        await unitOfWork.Payments.UpdateAsync(payment);
        await unitOfWork.SaveChangesAsync();

        return PaymentDto.FromEntity(payment);
    }
}