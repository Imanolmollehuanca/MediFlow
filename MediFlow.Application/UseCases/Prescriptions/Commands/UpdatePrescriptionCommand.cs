using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Prescriptions.Commands;

public record UpdatePrescriptionCommand(
    int PrescriptionId,
    UpdatePrescriptionDto Prescription
) : IRequest<bool>;

public class UpdatePrescriptionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePrescriptionCommand, bool>
{
    public async Task<bool> Handle(
        UpdatePrescriptionCommand request,
        CancellationToken cancellationToken)
    {
        var prescription = await unitOfWork.Prescriptions
            .GetByIdAsync(request.PrescriptionId);

        if (prescription is null)
            return false;

        request.Prescription.ApplyTo(prescription);

        await unitOfWork.Prescriptions.UpdateAsync(prescription);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}