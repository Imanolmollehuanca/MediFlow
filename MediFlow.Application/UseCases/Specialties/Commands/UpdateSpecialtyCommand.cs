using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;

public record UpdateSpecialtyCommand(int SpecialtyId, string Name, string? Description ) : IRequest<bool>;
internal sealed class UpdateSpecialtyCommandHandler : IRequestHandler<UpdateSpecialtyCommand, bool>
{
    private readonly IGenericRepository<Specialty> _repository;
    public UpdateSpecialtyCommandHandler(I­GenericRepository<Specialty> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("El nombre de la especialidad es obligatorio.");
        var specialty = await _repository.GetByIdAsync(request.SpecialtyId);
        if (specialty is null || !specialty.IsActive)
            return false;
        specialty.Name = request.Name.Trim();
        specialty.Description = request.Description;
        await _repository.UpdateAsync(specialty);
        return true;
    }
}