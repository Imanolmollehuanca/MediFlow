using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record UpdatePatientCommand(int PatientId, string DocumentType, string DocumentNumber, string FirstName, string LastName,
    DateOnly BirthDate, string Gender, string Phone, string? Email, string? Address ) : IRequest<bool>;

internal sealed class UpdatePatientCommandHandler
    : IRequestHandler<UpdatePatientCommand, bool>
{
    private readonly IGenericRepository<Patient> _repository;

    public UpdatePatientCommandHandler(
        IGenericRepository<Patient> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(request.PatientId);

        if (patient is null)
        {
            return false;
        }

        patient.DocumentType = request.DocumentType;
        patient.DocumentNumber = request.DocumentNumber;
        patient.FirstName = request.FirstName;
        patient.LastName = request.LastName;
        patient.BirthDate = request.BirthDate;
        patient.Gender = request.Gender;
        patient.Phone = request.Phone;
        patient.Email = request.Email;
        patient.Address = request.Address;

        await _repository.UpdateAsync(patient);

        return true;
    }
}