using MediFlow.Application.Dtos;
using MediFlow.Application.Interfaces;
using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Users;

public class UpdateUserUseCase(IUnitOfWork uow, IPasswordHasher hasher)
{
    public async Task<UserResponse> ExecuteAsync(int userId, UpdateUserRequest request, CancellationToken ct = default)
    {
        var user = await uow.Users.GetByIdAsync(userId, ct)
                   ?? throw new NotFoundException($"No se encontró el usuario con id {userId}.");

        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.Username.Trim();

        if (await uow.Users.ExistsByEmailAsync(email, userId, ct))
            throw new ConflictException($"El correo '{email}' ya está registrado.");

        if (await uow.Users.ExistsByUsernameAsync(username, userId, ct))
            throw new ConflictException($"El nombre de usuario '{username}' ya está en uso.");

        user.Username = username;
        user.Email = email;
        user.IsActive = request.IsActive;

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
            user.PasswordHash = hasher.Hash(request.NewPassword);

        await uow.SaveChangesAsync(ct);

        return user.ToResponse();
    }
}