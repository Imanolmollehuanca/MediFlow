using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Rooms.Commands;

public record CreateRoomCommand(CreateRoomDto Dto) : IRequest<RoomDto>;

internal sealed class CreateRoomCommandHandler
    : IRequestHandler<CreateRoomCommand, RoomDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoomDto> Handle(
        CreateRoomCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.RoomNumber))
        {
            throw new ArgumentException("El número de la sala es obligatorio.");
        }

        if (await _unitOfWork.Rooms.ExistsByRoomNumberAsync(
                dto.RoomNumber, cancellationToken: cancellationToken))
        {
            throw new ArgumentException("Ya existe una sala con ese número.");
        }

        var room = dto.ToEntity();

        await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();

        return RoomDto.FromEntity(room);
    }
}
