using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.PrescriptionDetails.Commands;


public record UpdatePrescriptionDetailCommand(
    int PrescriptionDetailId,
    UpdatePrescriptionDetailDto PrescriptionDetail
) : IRequest<bool>;

public class UpdatePrescriptionDetailCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePrescriptionDetailCommand, bool>
{
    public async Task<bool> Handle(
        UpdatePrescriptionDetailCommand request,
        CancellationToken cancellationToken)
    {
        var detail = await unitOfWork.PrescriptionDetails
            .GetByIdAsync(request.PrescriptionDetailId);

        if (detail is null)
            return false;

        var dto = request.PrescriptionDetail;

        if (string.IsNullOrWhiteSpace(dto.Dosage) ||
            string.IsNullOrWhiteSpace(dto.Frequency))
        {
            throw new ArgumentException(
                "La dosis y frecuencia son obligatorias.");
        }

        if (dto.DurationDays <= 0 || dto.Quantity <= 0)
            throw new ArgumentException(
                "La duración y cantidad deben ser mayores que cero.");

        var prescription = await unitOfWork.Prescriptions
            .GetByIdAsync(detail.PrescriptionId);

        if (prescription is null)
            throw new KeyNotFoundException("La receta no existe.");

        if (!prescription.IsActive)
            throw new InvalidOperationException(
                "No se puede modificar una receta inactiva.");

        var medication = await unitOfWork.Medications
            .GetByIdAsync(dto.MedicationId);

        if (medication is null)
            throw new KeyNotFoundException("El medicamento no existe.");

        if (!medication.IsActive)
            throw new InvalidOperationException(
                "El medicamento está inactivo.");

        var details = await unitOfWork.PrescriptionDetails.GetAllAsync();

        var exists = details.Any(d =>
            d.PrescriptionDetailId != request.PrescriptionDetailId &&
            d.PrescriptionId == detail.PrescriptionId &&
            d.MedicationId == dto.MedicationId);

        if (exists)
            throw new InvalidOperationException(
                "Este medicamento ya está registrado en la receta.");

        dto.ApplyTo(detail);

        await unitOfWork.PrescriptionDetails.UpdateAsync(detail);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}