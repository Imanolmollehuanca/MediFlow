using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Users;

public class DeleteUserUseCase(IUnitOfWork uow)
{
    public async Task ExecuteAsync(int userId, CancellationToken ct = default)
    {
        var user = await uow.Users.GetByIdAsync(userId, ct)
                   ?? throw new NotFoundException($"No se encontró el usuario con id {userId}.");

        uow.Users.Remove(user);
        await uow.SaveChangesAsync(ct);
    }
}