using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;

public record UpdateSpecialtyCommand(int SpecialtyId, UpdateSpecialtyDto Dto) : IRequest<bool>;

internal sealed class UpdateSpecialtyCommandHandler
    : IRequestHandler<UpdateSpecialtyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSpecialtyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdateSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("El nombre de la especialidad es obligatorio.");

        var specialty = await _unitOfWork.Specialties.GetByIdAsync(request.SpecialtyId);

        if (specialty is null || !specialty.IsActive)
            return false;

        dto.ApplyTo(specialty);

        await _unitOfWork.Specialties.UpdateAsync(specialty);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
