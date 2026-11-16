using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Roles.Queries;

public record GetRoleByIdQuery(int RoleId) : IRequest<RoleDto?>;

public class GetRoleByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    public async Task<RoleDto?> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(request.RoleId);

        if (role is null)
            return null;

        return RoleDto.FromEntity(role);
    }
}