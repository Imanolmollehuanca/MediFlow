using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.PrescriptionDetails.Commands;

public record DeletePrescriptionDetailCommand(int PrescriptionDetailId)
    : IRequest<bool>;

public class DeletePrescriptionDetailCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePrescriptionDetailCommand, bool>
{
    public async Task<bool> Handle(
        DeletePrescriptionDetailCommand request,
        CancellationToken cancellationToken)
    {
        var detail = await unitOfWork.PrescriptionDetails
            .GetByIdAsync(request.PrescriptionDetailId);

        if (detail is null)
            return false;

        throw new InvalidOperationException(
            "No está permitido eliminar detalles de recetas médicas.");
    }
}