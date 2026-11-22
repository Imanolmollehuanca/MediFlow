using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record DeleteDoctorCommand(int DoctorId) : IRequest<bool>;

internal sealed class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDoctorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeleteDoctorCommand request,
        CancellationToken cancellationToken)
    {
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            return false;

        doctor.IsActive = false;
        await _unitOfWork.Doctors.UpdateAsync(doctor);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
