using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.DoctorRooms.Queries;

public record GetDoctorRoomsQuery(int? DoctorId = null, int? RoomId = null)
    : IRequest<IEnumerable<DoctorRoomDto>>;

internal sealed class GetDoctorRoomsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetDoctorRoomsQuery, IEnumerable<DoctorRoomDto>>
{
    public async Task<IEnumerable<DoctorRoomDto>> Handle(
        GetDoctorRoomsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DoctorId is <= 0 || request.RoomId is <= 0)
        {
            throw new ArgumentException("Los filtros de médico y sala deben tener IDs positivos.");
        }

        var assignments = await unitOfWork.DoctorRooms.GetAssignmentsAsync(
            request.DoctorId, request.RoomId, cancellationToken);

        return assignments.Select(DoctorRoomDto.FromEntity).ToList();
    }
}
