using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record UpdatePatientCommand(
    int PatientId,
    UpdatePatientDto Dto
) : IRequest<bool>;

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
            return false;

        var dto = request.Dto;

        patient.DocumentType = dto.DocumentType;
        patient.DocumentNumber = dto.DocumentNumber;
        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;
        patient.BirthDate = dto.BirthDate;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;
        patient.Address = dto.Address;

        await _repository.UpdateAsync(patient);

        return true;
    }
}