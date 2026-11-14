using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.MedicalRecords.Commands;

public record DeleteMedicalRecordCommand(int MedicalRecordId) : IRequest<bool>;

internal sealed class DeleteMedicalRecordCommandHandler(
    IGenericRepository<MedicalRecord> records
) : IRequestHandler<DeleteMedicalRecordCommand, bool>
{
    public async Task<bool> Handle(
        DeleteMedicalRecordCommand request,
        CancellationToken cancellationToken)
    {
        var medicalRecord = await records.GetByIdAsync(request.MedicalRecordId);

        if (medicalRecord is null)
            return false;

        await records.DeleteAsync(medicalRecord);

        return true;
    }
}