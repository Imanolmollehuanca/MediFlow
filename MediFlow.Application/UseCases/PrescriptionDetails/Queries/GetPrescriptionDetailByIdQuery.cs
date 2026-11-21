using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.PrescriptionDetails.Queries;

public record GetPrescriptionDetailByIdQuery(int PrescriptionDetailId)
    : IRequest<PrescriptionDetailDto?>;

public class GetPrescriptionDetailByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPrescriptionDetailByIdQuery, PrescriptionDetailDto?>
{
    public async Task<PrescriptionDetailDto?> Handle(
        GetPrescriptionDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        var detail = await unitOfWork.PrescriptionDetails
            .GetByIdAsync(request.PrescriptionDetailId);

        return detail is null ? null : new PrescriptionDetailDto(detail);
    }
}