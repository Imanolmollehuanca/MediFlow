using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;
public record CreateSpecialtyCommand(string Name, string? Description ) : IRequest<int>;
internal sealed class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, int>
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
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("El nombre de la especialidad es obligatorio.");
        var specialty = new Specialty
        {
            Name = request.Name.Trim(),
            Description = request.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(specialty);
        return specialty.SpecialtyId;
    }
}