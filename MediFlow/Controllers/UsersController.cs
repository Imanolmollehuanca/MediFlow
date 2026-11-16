using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Users.Commands;
using MediFlow.Application.UseCases.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));

        return user is null
            ? NotFound(new { message = "Usuario no encontrado." })
            : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var user = await mediator.Send(new CreateUserCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.UserId },
            user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        var user = await mediator.Send(new UpdateUserCommand(id, dto));

        return user is null
            ? NotFound(new { message = "Usuario no encontrado." })
            : Ok(user);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(
        int id,
        ChangeUserStatusRequest request)
    {
        var user = await mediator.Send(
            new ChangeUserStatusCommand(id, request.IsActive));

        return user is null
            ? NotFound(new { message = "Usuario no encontrado." })
            : Ok(user);
    }

    [HttpPost("{id:int}/roles/{roleId:int}")]
    public async Task<IActionResult> AssignRole(int id, int roleId)
    {
        var user = await mediator.Send(new AssignRoleCommand(id, roleId));

        return user is null
            ? NotFound(new { message = "Usuario no encontrado." })
            : Ok(user);
    }

    [HttpDelete("{id:int}/roles/{roleId:int}")]
    public async Task<IActionResult> RemoveRole(int id, int roleId)
    {
        var user = await mediator.Send(new RemoveRoleCommand(id, roleId));

        return user is null
            ? NotFound(new { message = "Usuario no encontrado." })
            : Ok(user);
    }
}

public record ChangeUserStatusRequest(bool IsActive);
