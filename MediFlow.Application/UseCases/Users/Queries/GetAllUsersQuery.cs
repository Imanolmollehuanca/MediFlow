using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Users.Queries;

public record GetAllUsersQuery() : IRequest<List<UserDto>>;

public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await unitOfWork.Users.GetAllAsync();

        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var userWithRoles = await unitOfWork.Users
                .GetWithRolesAsync(user.UserId);

            if (userWithRoles is null)
                continue;

            result.Add(UserDto.FromEntity(userWithRoles));
        }

        return result;
    }
}