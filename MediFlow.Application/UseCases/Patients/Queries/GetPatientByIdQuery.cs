using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Queries;

public record GetPatientByIdQuery(int PatientId)
    : IRequest<PatientDto?>;

internal sealed class GetPatientByIdQueryHandler
    : IRequestHandler<GetPatientByIdQuery, PatientDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPatientByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PatientDto?> Handle(
        GetPatientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);

        return patient is null ? null : PatientDto.FromEntity(patient);
    }
}