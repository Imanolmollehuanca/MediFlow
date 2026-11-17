using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record DeletePatientCommand(int PatientId) : IRequest<bool>;

internal sealed class DeletePatientCommandHandler
    : IRequestHandler<DeletePatientCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePatientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeletePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
        if (patient is null || !patient.IsActive)
            return false;

        patient.IsActive = false;

        await _unitOfWork.Patients.UpdateAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}