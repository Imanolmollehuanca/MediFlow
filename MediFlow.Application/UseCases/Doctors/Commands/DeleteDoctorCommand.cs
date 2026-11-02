using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record DeleteDoctorCommand(int DoctorId) : IRequest<bool>;

internal sealed class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, bool>
{
    private readonly IGenericRepository<Doctor> _repository;

    public DeleteDoctorCommandHandler(IGenericRepository<Doctor> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteDoctorCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await _repository.GetByIdAsync(request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            return false;

        doctor.IsActive = false;
        await _repository.UpdateAsync(doctor);
        return true;
    }
}