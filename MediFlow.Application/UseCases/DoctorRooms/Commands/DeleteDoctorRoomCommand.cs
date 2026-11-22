using MediatR;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.DoctorRooms.Commands;

public record DeleteDoctorRoomCommand(int DoctorRoomId) : IRequest<bool>;

internal sealed class DeleteDoctorRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDoctorRoomCommand, bool>
{
    public async Task<bool> Handle(
        DeleteDoctorRoomCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await unitOfWork.DoctorRooms.GetByIdAsync(request.DoctorRoomId);
        if (assignment is null)
            return false;

        if (assignment.IsActive)
        {
            assignment.IsActive = false;
            await unitOfWork.DoctorRooms.UpdateAsync(assignment);
            await unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
