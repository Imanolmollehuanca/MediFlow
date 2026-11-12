using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.MedicalRecords.Commands;

public record CreateMedicalRecordCommand(CreateMedicalRecordDto Dto) : IRequest<int>;

internal sealed class CreateMedicalRecordCommandHandler(
    IGenericRepository<MedicalRecord> records,
    IGenericRepository<Patient> patients
) : IRequestHandler<CreateMedicalRecordCommand, int>
{
    public async Task<int> Handle(
        CreateMedicalRecordCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var patient = await patients.GetByIdAsync(dto.PatientId);

        if (patient is null || !patient.IsActive)
            throw new ArgumentException("El paciente no existe o está inactivo.");

        var medicalRecord = dto.ToEntity();

        await records.AddAsync(medicalRecord);

        return medicalRecord.MedicalRecordId;
    }
}