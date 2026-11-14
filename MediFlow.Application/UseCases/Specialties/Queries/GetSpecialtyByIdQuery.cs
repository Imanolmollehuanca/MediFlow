using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Queries;

public record GetSpecialtyByIdQuery(int SpecialtyId) : IRequest<SpecialtyDto?>;

internal sealed class GetSpecialtyByIdQueryHandler
    : IRequestHandler<GetSpecialtyByIdQuery, SpecialtyDto?>
{
    private readonly IGenericRepository<Specialty> _repository;

    public GetSpecialtyByIdQueryHandler(
        IGenericRepository<Specialty> repository)
    {
        _repository = repository;
    }

    public async Task<SpecialtyDto?> Handle(
        GetSpecialtyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var specialty = await _repository.GetByIdAsync(request.SpecialtyId);

        if (specialty is null)
            return null;

        return new SpecialtyDto(specialty);
    }
}