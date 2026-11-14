using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Queries;

public record GetPatientByIdQuery(int PatientId)
    : IRequest<PatientDto?>;

internal sealed class GetPatientByIdQueryHandler
    : IRequestHandler<GetPatientByIdQuery, PatientDto?>
{
    private readonly IGenericRepository<Patient> _repository;

    public GetPatientByIdQueryHandler(
        IGenericRepository<Patient> repository)
    {
        _repository = repository;
    }

    public async Task<PatientDto?> Handle(
        GetPatientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(request.PatientId);

        if (patient is null)
        {
            return null;
        }

        return new PatientDto(patient);
    }
}