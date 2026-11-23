using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Queries;

public record GetSpecialtyByIdQuery(int SpecialtyId) : IRequest<SpecialtyDto?>;

internal sealed class GetSpecialtyByIdQueryHandler
    : IRequestHandler<GetSpecialtyByIdQuery, SpecialtyDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSpecialtyByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SpecialtyDto?> Handle(
        GetSpecialtyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var specialty = await _unitOfWork.Specialties.GetByIdAsync(request.SpecialtyId);

        return specialty is null
            ? null
            : SpecialtyDto.FromEntity(specialty);
    }
}
