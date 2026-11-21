using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Medications.Queries;

public record GetMedicationByIdQuery(int MedicationId)
    : IRequest<MedicationDto?>;

public class GetMedicationByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetMedicationByIdQuery, MedicationDto?>
{
    public async Task<MedicationDto?> Handle(
        GetMedicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var medication = await unitOfWork.Medications
            .GetByIdAsync(request.MedicationId);

        return medication is null
            ? null
            : new MedicationDto(medication);
    }
}