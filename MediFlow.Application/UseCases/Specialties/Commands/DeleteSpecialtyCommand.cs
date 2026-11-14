using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;

public record DeleteSpecialtyCommand(int SpecialtyId) : IRequest<bool>;
internal sealed class DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand, bool>
{
    private readonly IGenericRepository<Specialty> _repository;
    public DeleteSpecialtyCommandHandler(IGenericRepository<Specialty> repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(
        DeleteSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var specialty = await _repository.GetByIdAsync(request.SpecialtyId);
        if (specialty is null || !specialty.IsActive)
            return false;
        specialty.IsActive = false;
        await _repository.UpdateAsync(specialty);
        return true;
    }
}