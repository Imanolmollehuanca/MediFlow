using MediFlow.Application.Dtos;
using MediFlow.Domain.Interfaces;

namespace MediFlow.Application.UseCases.Users;


public class GetAllUsersUseCase(IUnitOfWork uow)
{
    public async Task<IReadOnlyList<UserResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var users = await uow.Users.GetAllAsync(ct);
        return users.Select(u => u.ToResponse()).ToList();
    }
}