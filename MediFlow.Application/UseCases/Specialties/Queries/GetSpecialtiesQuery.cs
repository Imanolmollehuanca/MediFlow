using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Queries;

public record GetSpecialtiesQuery() : IRequest<IEnumerable<SpecialtyDto>>;

internal sealed class GetSpecialtiesQueryHandler
    : IRequestHandler<GetSpecialtiesQuery, IEnumerable<SpecialtyDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSpecialtiesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SpecialtyDto>> Handle(
        GetSpecialtiesQuery request,
        CancellationToken cancellationToken)
    {
        var specialties = await _unitOfWork.Specialties.GetAllAsync();

        return specialties.Select(SpecialtyDto.FromEntity).ToList();
    }
}
