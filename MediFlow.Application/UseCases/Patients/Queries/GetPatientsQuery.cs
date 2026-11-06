using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Patients.Queries;

public record GetPatientsQuery()
    : IRequest<IEnumerable<PatientDto>>;

internal sealed class GetPatientsQueryHandler
    : IRequestHandler<GetPatientsQuery, IEnumerable<PatientDto>>
{
    private readonly IGenericRepository<Patient> _repository;

    public GetPatientsQueryHandler(
        IGenericRepository<Patient> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PatientDto>> Handle(
        GetPatientsQuery request,
        CancellationToken cancellationToken)
    {
        var patients = await _repository.GetAllAsync();

        return patients.Select(patient => new PatientDto(patient));
    }
}