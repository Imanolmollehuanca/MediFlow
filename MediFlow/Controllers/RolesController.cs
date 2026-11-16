using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Roles.Commands;
using MediFlow.Application.UseCases.Roles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Admin")]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await mediator.Send(new GetAllRolesQuery());
        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await mediator.Send(new GetRoleByIdQuery(id));

        return role is null
            ? NotFound(new { message = "Rol no encontrado." })
            : Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDto dto)
    {
        var role = await mediator.Send(new CreateRoleCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = role.RoleId },
            role);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateRoleDto dto)
    {
        var role = await mediator.Send(new UpdateRoleCommand(id, dto));

        return role is null
            ? NotFound(new { message = "Rol no encontrado." })
            : Ok(role);
    }
}