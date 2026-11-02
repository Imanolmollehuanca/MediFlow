using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Patients.Queries;

public record GetPatientByIdQuery(int PatientId) : IRequest<PatientDto?>;
internal sealed class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDto?>
{
    private readonly IGenericRepository<Patient> _repository;
    public GetPatientByIdQueryHandler(I­GenericRepository<Patient> repository)
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
        return new PatientDto(
            patient.PatientId,
            patient.DocumentType,
            patient.DocumentNumber,
            patient.FirstName,
            patient.LastName,
            patient.BirthDate,
            patient.Gender,
            patient.Phone,
            patient.Email,
            patient.Address,
            patient.IsActive,
            patient.CreatedAt
        );
    }
}