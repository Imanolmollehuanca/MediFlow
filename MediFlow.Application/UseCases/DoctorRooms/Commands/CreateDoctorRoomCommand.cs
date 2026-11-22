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
        var assignment = request.Dto.ToEntity();
        await unitOfWork.DoctorRooms.AddAsync(assignment);
        await unitOfWork.SaveChangesAsync();

        return DoctorRoomDto.FromEntity(assignment);
    }
}
