using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Rooms.Commands;

public record UpdateRoomCommand(int RoomId, UpdateRoomDto Dto) : IRequest<bool>;

internal sealed class UpdateRoomCommandHandler
    : IRequestHandler<UpdateRoomCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdateRoomCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.RoomNumber))
        {
            throw new ArgumentException("El número de la sala es obligatorio.");
        }

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);

        if (room is null || !room.IsActive)
            return false;

        if (await _unitOfWork.Rooms.ExistsByRoomNumberAsync(
                dto.RoomNumber, request.RoomId, cancellationToken))
        {
            throw new ArgumentException("Ya existe otra sala con ese número.");
        }

        dto.ApplyTo(room);

        await _unitOfWork.Rooms.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
