using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;

public record UpdateSpecialtyCommand(int SpecialtyId, UpdateSpecialtyDto Dto) : IRequest<bool>;

internal sealed class UpdateSpecialtyCommandHandler
    : IRequestHandler<UpdateSpecialtyCommand, bool>
{
    private readonly IGenericRepository<Specialty> _repository;

    public UpdateSpecialtyCommandHandler(IGenericRepository<Specialty> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("El nombre de la especialidad es obligatorio.");

        var specialty = await _repository.GetByIdAsync(request.SpecialtyId);

        if (specialty is null || !specialty.IsActive)
            return false;

        specialty.Name = dto.Name.Trim();
        specialty.Description = dto.Description;

        await _repository.UpdateAsync(specialty);

        return true;
    }
}