using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Payments.Commands;

public record CreatePaymentCommand(CreatePaymentDto Dto)
    : IRequest<PaymentDto>;

public class CreatePaymentCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    public async Task<PaymentDto> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (dto.AppointmentId <= 0)
            throw new ArgumentException("La cita es obligatoria.");

        if (dto.Amount <= 0)
            throw new ArgumentException("El monto debe ser mayor que cero.");

        if (string.IsNullOrWhiteSpace(dto.PaymentMethod))
            throw new ArgumentException("El método de pago es obligatorio.");

        var appointment = await unitOfWork.Appointments
            .GetByIdAsync(dto.AppointmentId);

        if (appointment is null)
            throw new ArgumentException("La cita no existe.");

        var payment = new Payment
        {
            AppointmentId = dto.AppointmentId,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod.Trim(),
            TransactionReference = dto.TransactionReference?.Trim(),
            Status = "Pending",
            PaymentDate = DateTime.UtcNow
        };

        await unitOfWork.Payments.AddAsync(payment);
        await unitOfWork.SaveChangesAsync();

        return PaymentDto.FromEntity(payment);
    }
}