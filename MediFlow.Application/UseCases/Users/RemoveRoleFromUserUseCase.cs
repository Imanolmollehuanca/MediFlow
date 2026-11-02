using MediFlow.Domain.Exceptions;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Users;

public class RemoveRoleFromUserUseCase(IUnitOfWork uow)
{
    public async Task ExecuteAsync(int userId, int roleId, CancellationToken ct = default)
    {
        var userRole = await uow.UserRoles.GetAsync(userId, roleId, ct)
                       ?? throw new NotFoundException("El usuario no tiene asignado ese rol.");

        uow.UserRoles.Remove(userRole);
        await uow.SaveChangesAsync(ct);
    }
}