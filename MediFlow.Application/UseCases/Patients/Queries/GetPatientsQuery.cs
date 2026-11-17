using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Queries;

public record GetPatientsQuery(
    string? DocumentNumber = null,
    string? Name = null,
    bool? IsActive = null)
    : IRequest<IEnumerable<PatientDto>>;

internal sealed class GetPatientsQueryHandler
    : IRequestHandler<GetPatientsQuery, IEnumerable<PatientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPatientsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PatientDto>> Handle(
        GetPatientsQuery request,
        CancellationToken cancellationToken)
    {
        var patients = await _unitOfWork.Patients.SearchAsync(
            request.DocumentNumber,
            request.Name,
            request.IsActive);

        return patients.Select(PatientDto.FromEntity);
    }
}