using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Medications.Commands;

public record CreateMedicationCommand(CreateMedicationDto Medication)
    : IRequest<MedicationDto>;

public class CreateMedicationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMedicationCommand, MedicationDto>
{
    public async Task<MedicationDto> Handle(
        CreateMedicationCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Medication;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException(
                "El nombre del medicamento es obligatorio.");

        var medications = await unitOfWork.Medications.GetAllAsync();

        var exists = medications.Any(m =>
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
                "Ya existe un medicamento con ese nombre, presentación y concentración.");

        var medication = dto.ToEntity();

        await unitOfWork.Medications.AddAsync(medication);
        await unitOfWork.SaveChangesAsync();

        return new MedicationDto(medication);
    }
}