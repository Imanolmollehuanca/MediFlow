using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Queries;

public record GetDiagnosesQuery(int? MedicalRecordId = null)
    : IRequest<IEnumerable<DiagnosisDto>>;

internal sealed class GetDiagnosesQueryHandler(
    IUnitOfWork unitOfWork
) : IRequestHandler<GetDiagnosesQuery, IEnumerable<DiagnosisDto>>
{
    public async Task<IEnumerable<DiagnosisDto>> Handle(
        GetDiagnosesQuery request,
        CancellationToken cancellationToken)
    {
        var diagnoses = await unitOfWork.Diagnoses.GetAllAsync();

        if (request.MedicalRecordId.HasValue)
        {
            diagnoses = diagnoses.Where(d =>
                d.MedicalRecordId == request.MedicalRecordId.Value);
        }

        return diagnoses
            .Select(d => new DiagnosisDto(d))
            .ToList();
    }
}