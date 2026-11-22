using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Medications.Queries;

public record GetAllMedicationsQuery
    : IRequest<IEnumerable<MedicationDto>>;

public class GetAllMedicationsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllMedicationsQuery, IEnumerable<MedicationDto>>
{
    public async Task<IEnumerable<MedicationDto>> Handle(
        GetAllMedicationsQuery request,
        CancellationToken cancellationToken)
    {
        var medications = await unitOfWork.Medications.GetAllAsync();

        return medications.Select(m => new MedicationDto(m));
    }
}