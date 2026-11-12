using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Commands;

public record DeleteDiagnosisCommand(int DiagnosisId) : IRequest<bool>;

internal sealed class DeleteDiagnosisCommandHandler(
    IGenericRepository<Diagnosis> diagnoses
) : IRequestHandler<DeleteDiagnosisCommand, bool>
{
    public async Task<bool> Handle(
        DeleteDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        var diagnosis = await diagnoses.GetByIdAsync(request.DiagnosisId);
        if (diagnosis is null)
            return false;

        await diagnoses.DeleteAsync(diagnosis);
        return true;
    }
}