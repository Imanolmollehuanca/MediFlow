using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;
public record DeletePatientCommand(int PatientId) : IRequest<bool>;
internal sealed class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
{
    private readonly IGenericRepository<Patient> _repository;

    public DeletePatientCommandHandler(IGenericRepository<Patient> repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(
        DeletePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(request.PatientId);
        if (patient is null)
        {
            return false;
        }
        if (!patient.IsActive)
        {
            return false;
        }
        patient.IsActive = false;
        await _repository.UpdateAsync(patient);
        return true;
    }
}