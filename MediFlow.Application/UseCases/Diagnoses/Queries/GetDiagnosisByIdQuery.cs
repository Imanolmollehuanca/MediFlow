using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Queries;

public record GetDiagnosisByIdQuery(int DiagnosisId)
    : IRequest<DiagnosisDto?>;

internal sealed class GetDiagnosisByIdQueryHandler(
    IGenericRepository<Diagnosis> diagnoses
) : IRequestHandler<GetDiagnosisByIdQuery, DiagnosisDto?>
{
    public async Task<DiagnosisDto?> Handle(
        GetDiagnosisByIdQuery request,
        CancellationToken cancellationToken)
    {
        var diagnosis = await diagnoses.GetByIdAsync(request.DiagnosisId);

        if (diagnosis is null)
            return null;

        return new DiagnosisDto(diagnosis);
    }
}