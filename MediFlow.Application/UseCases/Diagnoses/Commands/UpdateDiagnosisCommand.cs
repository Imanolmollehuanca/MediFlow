using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Commands;

public record UpdateDiagnosisCommand(
    int DiagnosisId,
    UpdateDiagnosisDto Dto
) : IRequest<bool>;

internal sealed class UpdateDiagnosisCommandHandler(
    IGenericRepository<Diagnosis> diagnoses
) : IRequestHandler<UpdateDiagnosisCommand, bool>
{
    public async Task<bool> Handle(
        UpdateDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        var diagnosis = await diagnoses.GetByIdAsync(request.DiagnosisId);
        if (diagnosis is null)
            return false;

        if (string.IsNullOrWhiteSpace(request.Dto.Description))
            throw new ArgumentException("La descripción del diagnóstico es obligatoria.");

        request.Dto.ApplyTo(diagnosis);
        await diagnoses.UpdateAsync(diagnosis);

        return true;
    }
}