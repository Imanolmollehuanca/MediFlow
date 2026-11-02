using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record CreatePatientCommand(
    string DocumentType,
    string DocumentNumber,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string Gender,
    string Phone,
    string? Email,
    string? Address
) : IRequest<int>;

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
        var patient = new Patient
        {
            DocumentType = request.DocumentType,
            DocumentNumber = request.DocumentNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(patient);

        return patient.PatientId;
    }
}