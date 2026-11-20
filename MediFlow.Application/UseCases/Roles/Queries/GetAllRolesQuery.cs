using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Roles.Queries;

public record GetAllRolesQuery() : IRequest<List<RoleDto>>;

public class GetAllRolesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    public async Task<List<RoleDto>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await unitOfWork.Roles.GetAllAsync();

        return roles
            .Select(RoleDto.FromEntity)
            .ToList();
    }
}