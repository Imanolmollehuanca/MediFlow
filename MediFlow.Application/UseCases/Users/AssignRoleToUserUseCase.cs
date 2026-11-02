using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Users;

public class AssignRoleToUserUseCase(IUnitOfWork uow)
{
    public async Task<UserResponse> ExecuteAsync(int userId, int roleId, CancellationToken ct = default)
    {
        var user = await uow.Users.GetByIdAsync(userId, ct)
                   ?? throw new NotFoundException($"No se encontró el usuario con id {userId}.");

        _ = await uow.Roles.GetByIdAsync(roleId, ct)
            ?? throw new NotFoundException($"No se encontró el rol con id {roleId}.");

        if (await uow.UserRoles.GetAsync(userId, roleId, ct) is not null)
            throw new ConflictException("El usuario ya tiene asignado ese rol.");

        await uow.UserRoles.AddAsync(new UserRole { UserId = userId, RoleId = roleId }, ct);
        await uow.SaveChangesAsync(ct);

        var updated = await uow.Users.GetByIdAsync(userId, ct) ?? user;
        return updated.ToResponse();
    }
}