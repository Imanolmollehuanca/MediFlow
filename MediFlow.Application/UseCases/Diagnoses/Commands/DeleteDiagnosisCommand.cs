using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Commands;

public record DeleteDiagnosisCommand(int DiagnosisId) : IRequest<bool>;

internal sealed class DeleteDiagnosisCommandHandler(
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteDiagnosisCommand, bool>
{
    public async Task<bool> Handle(
        DeleteDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        var diagnosis = await unitOfWork.Diagnoses.GetByIdAsync(request.DiagnosisId);

        if (diagnosis is null)
            return false;

        await unitOfWork.Diagnoses.DeleteAsync(diagnosis);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}