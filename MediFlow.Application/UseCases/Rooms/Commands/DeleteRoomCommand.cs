using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Rooms.Commands;

public record DeleteRoomCommand(int RoomId) : IRequest<bool>;

internal sealed class DeleteRoomCommandHandler
    : IRequestHandler<DeleteRoomCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeleteRoomCommand request,
        CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);

        if (room is null || !room.IsActive)
            return false;

        if (await _unitOfWork.Rooms.HasActiveDoctorAssignmentsAsync(
                request.RoomId, cancellationToken))
        {
            throw new InvalidOperationException(
                "No se puede desactivar la sala porque tiene asignaciones activas de médicos.");
        }

        room.IsActive = false;

        await _unitOfWork.Rooms.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
