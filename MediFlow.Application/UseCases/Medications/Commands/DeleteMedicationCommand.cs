using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Medications.Commands;

public record DeleteMedicationCommand(int MedicationId)
    : IRequest<bool>;

public class DeleteMedicationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteMedicationCommand, bool>
{
    public async Task<bool> Handle(
        DeleteMedicationCommand request,
        CancellationToken cancellationToken)
    {
        var medication = await unitOfWork.Medications
            .GetByIdAsync(request.MedicationId);

        if (medication is null)
            return false;

        medication.IsActive = false;

        await unitOfWork.Medications.UpdateAsync(medication);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}