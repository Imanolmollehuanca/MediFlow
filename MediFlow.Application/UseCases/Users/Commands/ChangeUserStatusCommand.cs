using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Users.Commands;

public record ChangeUserStatusCommand(int UserId, bool IsActive)
    : IRequest<UserDto?>;

public class ChangeUserStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeUserStatusCommand, UserDto?>
{
    public async Task<UserDto?> Handle(
        ChangeUserStatusCommand request,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetWithRolesAsync(request.UserId);

        if (user is null)
            return null;

        user.IsActive = request.IsActive;

        await unitOfWork.Users.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();

        return UserDto.FromEntity(user);
    }
}