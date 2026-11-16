using MediatR;
using MediFlow.Application.UseCases.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
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

        if (user is null)
            return NotFound(new { message = "Usuario no encontrado." });

        return Ok(user);
    }
}