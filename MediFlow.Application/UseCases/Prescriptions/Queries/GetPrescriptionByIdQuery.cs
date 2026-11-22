using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Prescriptions.Queries;

public record GetPrescriptionByIdQuery(int PrescriptionId)
    : IRequest<PrescriptionDto?>;

public class GetPrescriptionByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPrescriptionByIdQuery, PrescriptionDto?>
{
    public async Task<PrescriptionDto?> Handle(
        GetPrescriptionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var prescription = await unitOfWork.Prescriptions
            .GetByIdAsync(request.PrescriptionId);

        return prescription is null
            ? null
            : new PrescriptionDto(prescription);
    }
}