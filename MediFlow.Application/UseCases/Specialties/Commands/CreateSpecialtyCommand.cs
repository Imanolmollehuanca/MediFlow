using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;

public record CreateSpecialtyCommand(CreateSpecialtyDto Dto) : IRequest<int>;

internal sealed class CreateSpecialtyCommandHandler
    : IRequestHandler<CreateSpecialtyCommand, int>
{
    private readonly IGenericRepository<Specialty> _repository;

    public CreateSpecialtyCommandHandler(IGenericRepository<Specialty> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(
        CreateSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("El nombre de la especialidad es obligatorio.");

        var specialty = new Specialty
        {
            Name = dto.Name.Trim(),
            Description = dto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(specialty);

        return specialty.SpecialtyId;
    }
}