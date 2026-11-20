using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Users.Commands;

public record UpdateUserCommand(int UserId, UpdateUserDto Dto)
    : IRequest<UserDto?>;

public class UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetWithRolesAsync(request.UserId);

        if (user is null)
            return null;

        var dto = request.Dto;
        var username = dto.Username?.Trim();
        var email = dto.Email?.Trim();

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El correo electrónico es obligatorio.");

        var existingUsername =
            await unitOfWork.Users.GetByUsernameAsync(username);

        if (existingUsername is not null &&
            existingUsername.UserId != request.UserId)
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya está registrado.");
        }

        var existingEmail =
            await unitOfWork.Users.GetByEmailAsync(email);

        if (existingEmail is not null &&
            existingEmail.UserId != request.UserId)
        {
            throw new InvalidOperationException(
                "El correo electrónico ya está registrado.");
        }

        user.Username = username;
        user.Email = email;

        await unitOfWork.Users.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();

        return UserDto.FromEntity(user);
    }
}