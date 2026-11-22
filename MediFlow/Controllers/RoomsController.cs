using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Application.UseCases.Rooms.Commands;
using MediFlow.Application.UseCases.Rooms.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;

    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoomDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var rooms = await _mediator.Send(new GetRoomsQuery(), cancellationToken);

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var room = await _mediator.Send(new GetRoomByIdQuery(id), cancellationToken);

        if (room is null)
            return RoomNotFound();

        return Ok(room);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoomDto>> Create(
        [FromBody] CreateRoomDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var room = await _mediator.Send(new CreateRoomCommand(dto), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = room.RoomId }, room);
        }
        catch (ArgumentException ex)
        {
            return Problem(title: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateRoomDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _mediator.Send(new UpdateRoomCommand(id, dto), cancellationToken);

            if (!updated)
                return RoomNotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return Problem(title: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteRoomCommand(id), cancellationToken);

            if (!deleted)
                return RoomNotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Problem(title: ex.Message, statusCode: StatusCodes.Status409Conflict);
        }
    }

    private ObjectResult RoomNotFound() => Problem(
        title: "Sala no encontrada.",
        statusCode: StatusCodes.Status404NotFound);
}
