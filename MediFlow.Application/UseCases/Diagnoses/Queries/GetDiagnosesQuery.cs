using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Queries;

public record GetDiagnosesQuery() : IRequest<IEnumerable<DiagnosisDto>>;
internal sealed class GetDiagnosesQueryHandler(
    IGenericRepository<Diagnosis> diagnoses
) : IRequestHandler<GetDiagnosesQuery, IEnumerable<DiagnosisDto>>
{
    public async Task<IEnumerable<DiagnosisDto>> Handle(
        GetDiagnosesQuery request,
        CancellationToken cancellationToken)
    {
        var diagnosisList = await diagnoses.GetAllAsync();
        return diagnosisList.Select(d => new DiagnosisDto(d)).ToList();
    }
}