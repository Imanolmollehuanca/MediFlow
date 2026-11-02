using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Queries;

public record GetSpecialtiesQuery() : IRequest<IEnumerable<SpecialtyDto>>;
internal sealed class GetSpecialtiesQueryHandler : IRequestHandler<GetSpecialtiesQuery, IEnumerable<SpecialtyDto>>
{
    private readonly IGenericRepository<Specialty> _repository;
    public GetSpecialtiesQueryHandler(IGenericRepository<Specialty> repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<SpecialtyDto>> Handle(
        GetSpecialtiesQuery request,
        CancellationToken cancellationToken)
    {
        var specialties = await _repository.GetAllAsync();
        return specialties.Select(s => new SpecialtyDto(
            s.SpecialtyId,
            s.Name,
            s.Description,
            s.IsActive,
            s.CreatedAt
        )).ToList();
    }
}