using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Users.Commands;

public record RemoveRoleCommand(int UserId, int RoleId)
    : IRequest<UserDto?>;

public class RemoveRoleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveRoleCommand, UserDto?>
{
    public async Task<UserDto?> Handle(
        RemoveRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user is null)
            return null;

        var userRole = await unitOfWork.UserRoles
            .GetByIdsAsync(request.UserId, request.RoleId);

        if (userRole is null)
            throw new InvalidOperationException(
                "El usuario no tiene asignado este rol.");

        await unitOfWork.UserRoles.DeleteAsync(userRole);
        await unitOfWork.SaveChangesAsync();

        var updatedUser = await unitOfWork.Users
            .GetWithRolesAsync(request.UserId);

        return updatedUser is null
            ? null
            : UserDto.FromEntity(updatedUser);
    }
}