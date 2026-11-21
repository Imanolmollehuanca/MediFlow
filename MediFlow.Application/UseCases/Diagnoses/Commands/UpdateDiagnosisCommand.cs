using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Commands;

public record UpdateDiagnosisCommand(
    int DiagnosisId,
    UpdateDiagnosisDto Dto
) : IRequest<bool>;

internal sealed class UpdateDiagnosisCommandHandler(
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateDiagnosisCommand, bool>
{
    public async Task<bool> Handle(
        UpdateDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        var diagnosis = await unitOfWork.Diagnoses.GetByIdAsync(request.DiagnosisId);

        if (diagnosis is null)
            return false;

        if (string.IsNullOrWhiteSpace(request.Dto.Description))
            throw new ArgumentException("La descripción del diagnóstico es obligatoria.");

        request.Dto.ApplyTo(diagnosis);

        await unitOfWork.Diagnoses.UpdateAsync(diagnosis);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}