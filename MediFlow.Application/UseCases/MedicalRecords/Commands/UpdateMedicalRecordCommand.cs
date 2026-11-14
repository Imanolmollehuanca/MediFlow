using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.MedicalRecords.Commands;

public record UpdateMedicalRecordCommand(
    int MedicalRecordId,
    UpdateMedicalRecordDto Dto
) : IRequest<bool>;

internal sealed class UpdateMedicalRecordCommandHandler(
    IGenericRepository<MedicalRecord> records
) : IRequestHandler<UpdateMedicalRecordCommand, bool>
{
    public async Task<bool> Handle(
        UpdateMedicalRecordCommand request,
        CancellationToken cancellationToken)
    {
        var medicalRecord = await records.GetByIdAsync(request.MedicalRecordId);

        if (medicalRecord is null)
            return false;

        request.Dto.ApplyTo(medicalRecord);

        await records.UpdateAsync(medicalRecord);

        return true;
    }
}