using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record CreatePatientCommand(CreatePatientDto Dto) : IRequest<int>;

internal sealed class CreatePatientCommandHandler
    : IRequestHandler<CreatePatientCommand, int>
{
    private readonly IGenericRepository<Patient> _repository;

    public CreatePatientCommandHandler(
        IGenericRepository<Patient> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        CreatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var patient = new Patient
        {
            DocumentType = dto.DocumentType,
            DocumentNumber = dto.DocumentNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(patient);

        return patient.PatientId;
    }
}