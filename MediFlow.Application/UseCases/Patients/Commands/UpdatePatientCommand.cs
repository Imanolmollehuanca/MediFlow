using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record UpdatePatientCommand(
    int PatientId,
    UpdatePatientDto Dto
) : IRequest<bool>;

internal sealed class UpdatePatientCommandHandler
    : IRequestHandler<UpdatePatientCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePatientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
        if (patient is null)
            return false;

        var dto = request.Dto;
        CreatePatientCommandHandler.ValidatePatientData(
            dto.DocumentType, dto.DocumentNumber, dto.FirstName, dto.LastName,
            dto.BirthDate, dto.Gender, dto.Phone, dto.Email);

        var exists = await _unitOfWork.Patients.ExistsByDocumentAsync(
            dto.DocumentType, dto.DocumentNumber, request.PatientId);
        if (exists)
            throw new InvalidOperationException($"Ya existe otro paciente registrado con {dto.DocumentType.Trim().ToUpper()} {dto.DocumentNumber.Trim()}.");

        patient.DocumentType = dto.DocumentType.Trim().ToUpper();
        patient.DocumentNumber = dto.DocumentNumber.Trim();
        patient.FirstName = dto.FirstName.Trim();
        patient.LastName = dto.LastName.Trim();
        patient.BirthDate = dto.BirthDate;
        patient.Gender = dto.Gender.Trim();
        patient.Phone = dto.Phone.Trim();
        patient.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        patient.Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim();
        patient.IsActive = dto.IsActive;

        await _unitOfWork.Patients.UpdateAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}