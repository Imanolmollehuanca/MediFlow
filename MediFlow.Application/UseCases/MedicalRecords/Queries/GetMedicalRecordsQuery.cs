using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.MedicalRecords.Queries;

public record GetMedicalRecordsQuery() : IRequest<IEnumerable<MedicalRecordDto>>;

internal sealed class GetMedicalRecordsQueryHandler(
    IGenericRepository<MedicalRecord> records
) : IRequestHandler<GetMedicalRecordsQuery, IEnumerable<MedicalRecordDto>>
{
    public async Task<IEnumerable<MedicalRecordDto>> Handle(
        GetMedicalRecordsQuery request,
        CancellationToken cancellationToken)
    {
        var medicalRecords = await records.GetAllAsync();

        return medicalRecords.Select(r => new MedicalRecordDto(r)).ToList();
    }
}