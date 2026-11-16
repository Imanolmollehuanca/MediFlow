using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Users.Queries;

public record GetUserByIdQuery(int UserId) : IRequest<UserDto?>;

public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users
            .GetWithRolesAsync(request.UserId);

        if (user is null)
            return null;

        return UserDto.FromEntity(user);
    }
}