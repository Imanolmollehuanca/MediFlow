using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.MedicalRecords.Queries;

public record GetMedicalRecordByIdQuery(int MedicalRecordId)
    : IRequest<MedicalRecordDto?>;

internal sealed class GetMedicalRecordByIdQueryHandler(
    IGenericRepository<MedicalRecord> records
) : IRequestHandler<GetMedicalRecordByIdQuery, MedicalRecordDto?>
{
    public async Task<MedicalRecordDto?> Handle(
        GetMedicalRecordByIdQuery request,
        CancellationToken cancellationToken)
    {
        var medicalRecord = await records.GetByIdAsync(request.MedicalRecordId);

        if (medicalRecord is null)
            return null;

        return new MedicalRecordDto(medicalRecord);
    }
}