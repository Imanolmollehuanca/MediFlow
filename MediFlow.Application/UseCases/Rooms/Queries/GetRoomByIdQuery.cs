using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Rooms.Queries;

public record GetRoomByIdQuery(int RoomId) : IRequest<RoomDto?>;

internal sealed class GetRoomByIdQueryHandler
    : IRequestHandler<GetRoomByIdQuery, RoomDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoomByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoomDto?> Handle(
        GetRoomByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.RoomId <= 0)
            return null;

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);

        return room is null
            ? null
            : RoomDto.FromEntity(room);
    }
}
