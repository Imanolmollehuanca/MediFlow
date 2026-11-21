using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Specialties.Commands;

public record CreateSpecialtyCommand(CreateSpecialtyDto Dto) : IRequest<int>;

internal sealed class CreateSpecialtyCommandHandler
    : IRequestHandler<CreateSpecialtyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpecialtyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateSpecialtyCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("El nombre de la especialidad es obligatorio.");

        var specialty = dto.ToEntity();

        await _unitOfWork.Specialties.AddAsync(specialty);
        await _unitOfWork.SaveChangesAsync();

        return specialty.SpecialtyId;
    }
}
