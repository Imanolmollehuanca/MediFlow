using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.DoctorRooms.Commands;

public record UpdateDoctorRoomCommand(int DoctorRoomId, UpdateDoctorRoomDto Dto) : IRequest<bool>;

internal sealed class UpdateDoctorRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateDoctorRoomCommand, bool>
{
    public async Task<bool> Handle(
        UpdateDoctorRoomCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await unitOfWork.DoctorRooms.GetByIdAsync(request.DoctorRoomId);
        if (assignment is null)
            return false;

        request.Dto.ApplyTo(assignment);
        await unitOfWork.DoctorRooms.UpdateAsync(assignment);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}
