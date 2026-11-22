using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Prescriptions.Commands;

public record DeletePrescriptionCommand(int PrescriptionId)
    : IRequest<bool>;

public class DeletePrescriptionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePrescriptionCommand, bool>
{
    public async Task<bool> Handle(
        DeletePrescriptionCommand request,
        CancellationToken cancellationToken)
    {
        var prescription = await unitOfWork.Prescriptions
            .GetByIdAsync(request.PrescriptionId);

        if (prescription is null)
            return false;

        prescription.IsActive = false;

        await unitOfWork.Prescriptions.UpdateAsync(prescription);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}