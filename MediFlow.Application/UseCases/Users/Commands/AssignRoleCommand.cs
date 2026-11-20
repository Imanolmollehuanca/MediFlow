using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Users.Commands;

public record AssignRoleCommand(int UserId, int RoleId)
    : IRequest<UserDto?>;

public class AssignRoleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AssignRoleCommand, UserDto?>
{
    public async Task<UserDto?> Handle(
        AssignRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user is null)
            return null;

        var role = await unitOfWork.Roles.GetByIdAsync(request.RoleId);

        if (role is null)
            throw new ArgumentException("El rol no existe.");

        var existingUserRole = await unitOfWork.UserRoles
            .GetByIdsAsync(request.UserId, request.RoleId);

        if (existingUserRole is not null)
            throw new InvalidOperationException(
                "El usuario ya tiene asignado este rol.");

        var userRole = new UserRole
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        await unitOfWork.UserRoles.AddAsync(userRole);
        await unitOfWork.SaveChangesAsync();

        var updatedUser = await unitOfWork.Users
            .GetWithRolesAsync(request.UserId);

        return updatedUser is null
            ? null
            : UserDto.FromEntity(updatedUser);
    }
}