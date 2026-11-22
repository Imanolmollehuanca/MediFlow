using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.PrescriptionDetails.Commands;

public record CreatePrescriptionDetailCommand(
    CreatePrescriptionDetailDto PrescriptionDetail
) : IRequest<PrescriptionDetailDto>;

public class CreatePrescriptionDetailCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePrescriptionDetailCommand, PrescriptionDetailDto>
{
    public async Task<PrescriptionDetailDto> Handle(
        CreatePrescriptionDetailCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.PrescriptionDetail;

        if (dto.PrescriptionId <= 0 || dto.MedicationId <= 0)
            throw new ArgumentException(
                "Los IDs de receta y medicamento deben ser válidos.");

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
            .GetByIdAsync(dto.PrescriptionId);

        if (prescription is null)
            throw new KeyNotFoundException("La receta no existe.");

        if (!prescription.IsActive)
            throw new InvalidOperationException(
                "No se pueden agregar medicamentos a una receta inactiva.");

        var medication = await unitOfWork.Medications
            .GetByIdAsync(dto.MedicationId);

        if (medication is null)
            throw new KeyNotFoundException("El medicamento no existe.");

        if (!medication.IsActive)
            throw new InvalidOperationException(
                "El medicamento está inactivo.");

        var details = await unitOfWork.PrescriptionDetails.GetAllAsync();

        var exists = details.Any(d =>
            d.PrescriptionId == dto.PrescriptionId &&
            d.MedicationId == dto.MedicationId);

        if (exists)
            throw new InvalidOperationException(
                "Este medicamento ya está registrado en la receta.");

        var detail = dto.ToEntity();

        await unitOfWork.PrescriptionDetails.AddAsync(detail);
        await unitOfWork.SaveChangesAsync();

        return new PrescriptionDetailDto(detail);
    }
}