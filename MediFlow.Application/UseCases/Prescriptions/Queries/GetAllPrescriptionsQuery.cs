using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Prescriptions.Queries;

public record GetAllPrescriptionsQuery
    : IRequest<IEnumerable<PrescriptionDto>>;

public class GetAllPrescriptionsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllPrescriptionsQuery, IEnumerable<PrescriptionDto>>
{
    public async Task<IEnumerable<PrescriptionDto>> Handle(
        GetAllPrescriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var prescriptions = await unitOfWork.Prescriptions.GetAllAsync();

        return prescriptions.Select(p => new PrescriptionDto(p));
    }
}