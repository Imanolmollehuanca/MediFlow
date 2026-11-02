using MediFlow.Application.Dtos;
using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Users;

public class GetUserByIdUseCase(IUnitOfWork uow)
{
    public async Task<UserResponse> ExecuteAsync(int userId, CancellationToken ct = default)
    {
        var user = await uow.Users.GetByIdAsync(userId, ct)
                   ?? throw new NotFoundException($"No se encontró el usuario con id {userId}.");

        return user.ToResponse();
    }
}