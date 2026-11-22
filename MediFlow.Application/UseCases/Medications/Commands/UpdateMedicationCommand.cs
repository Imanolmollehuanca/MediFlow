using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Medications.Commands;

public record UpdateMedicationCommand(
    int MedicationId,
    UpdateMedicationDto Medication
) : IRequest<bool>;

public class UpdateMedicationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMedicationCommand, bool>
{
    public async Task<bool> Handle(
        UpdateMedicationCommand request,
        CancellationToken cancellationToken)
    {
        var medication = await unitOfWork.Medications
            .GetByIdAsync(request.MedicationId);

        if (medication is null)
            return false;

        var dto = request.Medication;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException(
                "El nombre del medicamento es obligatorio.");

        var medications = await unitOfWork.Medications.GetAllAsync();

        var exists = medications.Any(m =>
            m.MedicationId != request.MedicationId &&
            string.Equals(
                m.Name.Trim(),
                dto.Name.Trim(),
                StringComparison.OrdinalIgnoreCase) &&
            string.Equals(
                m.Presentation?.Trim(),
                dto.Presentation?.Trim(),
                StringComparison.OrdinalIgnoreCase) &&
            string.Equals(
                m.Concentration?.Trim(),
                dto.Concentration?.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (exists)
            throw new InvalidOperationException(
                "Ya existe otro medicamento con esos mismos datos.");

        dto.ApplyTo(medication);

        await unitOfWork.Medications.UpdateAsync(medication);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}