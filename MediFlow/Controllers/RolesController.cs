using MediatR;
using MediFlow.Application.UseCases.Roles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
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

        if (role is null)
            return NotFound(new { message = "Rol no encontrado." });

        return Ok(role);
    }
}