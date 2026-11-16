using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Roles.Commands;

public record CreateRoleCommand(CreateRoleDto Dto) : IRequest<RoleDto>;

public class CreateRoleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRoleCommand, RoleDto>
{
    public async Task<RoleDto> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var name = dto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del rol es obligatorio.");

        if (await unitOfWork.Roles.GetByNameAsync(name) is not null)
            throw new InvalidOperationException(
                "Ya existe un rol con ese nombre.");

        var role = new Role
        {
            Name = name,
            Description = dto.Description?.Trim()
        };

        await unitOfWork.Roles.AddAsync(role);
        await unitOfWork.SaveChangesAsync();

        return RoleDto.FromEntity(role);
    }
}