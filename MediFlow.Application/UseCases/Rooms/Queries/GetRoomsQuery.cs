using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Rooms.Queries;

public record GetRoomsQuery() : IRequest<IEnumerable<RoomDto>>;

internal sealed class GetRoomsQueryHandler
    : IRequestHandler<GetRoomsQuery, IEnumerable<RoomDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoomsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoomDto>> Handle(
        GetRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await _unitOfWork.Rooms.GetAllAsync();

        return rooms.Select(RoomDto.FromEntity).ToList();
    }
}
