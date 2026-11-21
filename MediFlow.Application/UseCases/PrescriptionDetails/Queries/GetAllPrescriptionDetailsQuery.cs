using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.PrescriptionDetails.Queries;

public record GetAllPrescriptionDetailsQuery
    : IRequest<IEnumerable<PrescriptionDetailDto>>;

public class GetAllPrescriptionDetailsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllPrescriptionDetailsQuery, IEnumerable<PrescriptionDetailDto>>
{
    public async Task<IEnumerable<PrescriptionDetailDto>> Handle(
        GetAllPrescriptionDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var details = await unitOfWork.PrescriptionDetails.GetAllAsync();

        return details.Select(d => new PrescriptionDetailDto(d));
    }
}