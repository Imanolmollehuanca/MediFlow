using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.Port.Services;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Users.Commands;

public record CreateUserCommand(CreateUserDto Dto) : IRequest<UserDto>;

public class CreateUserCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordService passwordService)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var username = dto.Username?.Trim();
        var email = dto.Email?.Trim();

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El correo electrónico es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Password) ||
            dto.Password.Length < 8)
        {
            throw new ArgumentException(
                "La contraseña debe tener al menos 8 caracteres.");
        }

        if (await unitOfWork.Users.GetByUsernameAsync(username) is not null)
            throw new InvalidOperationException(
                "El nombre de usuario ya está registrado.");

        if (await unitOfWork.Users.GetByEmailAsync(email) is not null)
            throw new InvalidOperationException(
                "El correo electrónico ya está registrado.");

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordService.HashPassword(dto.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return UserDto.FromEntity(user);
    }
}