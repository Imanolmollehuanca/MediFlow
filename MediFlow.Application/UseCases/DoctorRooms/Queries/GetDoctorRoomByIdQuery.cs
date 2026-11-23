using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.DoctorRooms.Queries;

public record GetDoctorRoomByIdQuery(int DoctorRoomId) : IRequest<DoctorRoomDto?>;

internal sealed class GetDoctorRoomByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetDoctorRoomByIdQuery, DoctorRoomDto?>
{
    public async Task<DoctorRoomDto?> Handle(
        GetDoctorRoomByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DoctorRoomId <= 0)
            return null;

        var assignment = await unitOfWork.DoctorRooms.GetByIdAsync(request.DoctorRoomId);

        return assignment is null
            ? null
            : DoctorRoomDto.FromEntity(assignment);
    }
}
