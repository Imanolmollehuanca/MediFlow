using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;

public record DeleteSpecialtyCommand(int SpecialtyId) : IRequest<bool>;

internal sealed class DeleteSpecialtyCommandHandler
    : IRequestHandler<DeleteSpecialtyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpecialtyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeleteSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var specialty = await _unitOfWork.Specialties.GetByIdAsync(request.SpecialtyId);

        if (specialty is null || !specialty.IsActive)
            return false;

        if (await _unitOfWork.Specialties.HasActiveDoctorsAsync(
                request.SpecialtyId, cancellationToken))
        {
            throw new InvalidOperationException(
                "No se puede desactivar la especialidad porque tiene médicos activos asociados.");
        }

        specialty.IsActive = false;

        await _unitOfWork.Specialties.UpdateAsync(specialty);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
