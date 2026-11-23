using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.DoctorRooms.Commands;

public record CreateDoctorRoomCommand(CreateDoctorRoomDto Dto) : IRequest<DoctorRoomDto>;

internal sealed class CreateDoctorRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDoctorRoomCommand, DoctorRoomDto>
{
    public async Task<DoctorRoomDto> Handle(
        CreateDoctorRoomCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        await DoctorRoomValidation.ValidateAsync(unitOfWork, dto.DoctorId, dto.RoomId,
            isActive: true, excludedAssignmentId: null, cancellationToken);

        var assignment = dto.ToEntity();
        await unitOfWork.DoctorRooms.AddAsync(assignment);
        await unitOfWork.SaveChangesAsync();

        return DoctorRoomDto.FromEntity(assignment);
    }
}
