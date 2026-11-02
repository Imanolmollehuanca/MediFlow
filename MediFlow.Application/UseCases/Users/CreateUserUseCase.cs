using MediFlow.Application.Dtos;
using MediFlow.Application.Interfaces;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Users;

public class CreateUserUseCase(IUnitOfWork uow, IPasswordHasher hasher)
{
    public async Task<UserResponse> ExecuteAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.Username.Trim();

        if (await uow.Users.ExistsByEmailAsync(email, null, ct))
            throw new ConflictException($"El correo '{email}' ya está registrado.");

        if (await uow.Users.ExistsByUsernameAsync(username, null, ct))
            throw new ConflictException($"El nombre de usuario '{username}' ya está en uso.");

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = hasher.Hash(request.Password)
        };

        foreach (var roleId in (request.RoleIds ?? []).Distinct())
        {
            _ = await uow.Roles.GetByIdAsync(roleId, ct)
                ?? throw new NotFoundException($"No se encontró el rol con id {roleId}.");

            user.UserRoles.Add(new UserRole { RoleId = roleId });
        }

        await uow.Users.AddAsync(user, ct);
        await uow.SaveChangesAsync(ct);

        var created = await uow.Users.GetByIdAsync(user.UserId, ct) ?? user;
        return created.ToResponse();
    }
}